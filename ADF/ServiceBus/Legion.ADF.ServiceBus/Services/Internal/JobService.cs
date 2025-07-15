using Legion.ADF.ServiceBus.Services.Internal.Dto;
using Legion.ADF.ServiceBus.Settings;
using Legion.Database;
using Legion.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.ServiceBus.Services.Internal;

internal partial class JobService : IAsyncDisposable
{
	private readonly HostContext _hostContext;
	private readonly EnterpriseServiceBusOptions _options;
	private readonly AsyncServiceScope _serviceScope;
	private readonly ILogger<JobService> _logger;

	private readonly CancellationTokenSource _cancellationTokenSource;

	private Task? _workerTask;
	private bool _disposed;

	public JobContext JobContext { get; }

	public JobService(
		HostContext hostContext,
		JobContext jobContext,
		EnterpriseServiceBusOptions options,
		AsyncServiceScope serviceScope)
	{
		Throw.IfArgumentNull(hostContext);
		Throw.IfArgumentNull(jobContext);
		Throw.IfArgumentNull(serviceScope);

		_hostContext = hostContext;
		JobContext = jobContext;
		_options = options;
		_serviceScope = serviceScope;

		_logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<JobService>>();

		_cancellationTokenSource = new CancellationTokenSource();
	}

	private IServiceBusUnitOfWork CreateUnitOfWork(IServiceProvider serviceProvider)
		=> UnitOfWorkFactory<IServiceBusUnitOfWork, ConnectionStringProvider>.CreateUnitOfWorkWithoutTransaction(
			serviceProvider,
			_options.StoreId);

	private IResult CreateLogHost(
		IScopeContext scopeContext,
		IServiceBusUnitOfWork uow,
		Guid idHost,
		string code,
		bool force,
		ILogMessage logMessage,
		bool isRunning)
	{
		if (!force && !_logger.IsEnabled((LogLevel)logMessage.IdLogLevel))
			return new ResultBuilder().Build();

		var createdResult = Model.HostLog.Create(
			scopeContext,
			idHost,
			code,
			logMessage,
			isRunning);

		if (!createdResult.HasErrorOrNullData)
			uow.HostLogRepository.Add(scopeContext, createdResult.Data!);

		return createdResult;
	}

	private async Task WriteNoJobLog(
		IScopeContext scopeContext,
		IServiceBusUnitOfWork uow,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		CreateLogHost(
			scopeContext,
			uow,
			_hostContext.IdHost ?? Guid.Empty,
			HostService.Codes.NoJob,
			force: true,
			new LogMessageBuilder(scopeContext, errorCode: null)
				.LogLevel(LogLevel.Error)
				.InternalMessage($"Host {_options.HostName}: No job with id {JobContext.IdJob} found.")
				.Build(),
			isRunning: true)
			.ThrowIfErrorOrNullData(scopeContext, null, true);

		var saveHostLogResult = await uow.SaveAsync(scopeContext, cancellationToken);
		saveHostLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);
	}

	public async Task<bool> TryStartAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName)
			.AddContextProperty(nameof(JobContext.JobName), JobContext.JobName);

		await using var uow = CreateUnitOfWork(_serviceScope.ServiceProvider);
		var job = await uow.JobRepository
			.GetJobById(new Queries.Job.GetJobByIdQuery(JobContext.IdJob, false, false, true, null))
			.ToResultAsync(scopeContext, cancellationToken);

		if (job == null)
		{
			await WriteNoJobLog(scopeContext, uow, cancellationToken);
			return false;
		}

		if (job.JobActivity == null)
		{
			var createResult = Model.JobActivity.Create(scopeContext, _options.HostName, job, _hostContext.IdHost!.Value);
			createResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		}

		var disabling = await DisablingAsync(
			scopeContext,
			job,
			uow,
			_serviceScope.ServiceProvider,
			cancellationToken);

		if (disabling)
			return false;

		var startResult = job.JobActivity!.Start(scopeContext, _options.HostName);
		startResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		var saveLogResult = await uow.SaveAsync(scopeContext, cancellationToken);
		saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		_workerTask = Task.Run(() => 
			ExecuteAsync(
				scopeContext,
				_cancellationTokenSource.Token));

		return true;
	}

	private async Task<bool> DisablingAsync(
		IScopeContext scopeContext,
		Model.Job job,
		IServiceBusUnitOfWork uow,
		IServiceProvider serviceProvider,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName)
			.AddContextProperty(nameof(JobContext.JobName), JobContext.JobName);

		if (!job.RequestedToDisable)
			return false;

		var disablingResult = job.JobActivity.Disabling(scopeContext, _options.HostName);
		disablingResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		var canDisabled = false;

		var disableResult = job.JobActivity.Disable(scopeContext, _options.HostName);
		disableResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		canDisabled = disableResult.Data;

		var disableLogResult = await uow.SaveAsync(scopeContext, cancellationToken);
		disableLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		if (canDisabled)
		{
			_hostContext.RunningJobs.TryRemove(JobContext.IdJob, out _);
			_cancellationTokenSource.Cancel();
		}

		return true;
	}

	public async Task StopAsync(IScopeContext scopeContext)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName)
			.AddContextProperty(nameof(JobContext.JobName), JobContext.JobName);

		_hostContext.RunningJobs.TryRemove(JobContext.IdJob, out _);

		_cancellationTokenSource.Cancel();
		if (_workerTask != null)
			await _workerTask;
	}

	private async Task ExecuteAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			scopeContext = scopeContext.CreateNew()
				.AddContextProperty(nameof(_options.HostName), _options.HostName)
				.AddContextProperty(nameof(JobContext.JobName), JobContext.JobName);

			await using (var uow = CreateUnitOfWork(_serviceScope.ServiceProvider))
			{
				var job = await uow.JobRepository
					.GetJobById(new Queries.Job.GetJobByIdQuery(JobContext.IdJob, false, false, true, null))
					.ToResultAsync(scopeContext, cancellationToken);

				if (job == null)
				{
					await WriteNoJobLog(scopeContext, uow, cancellationToken);
					break;
				}

				var disabling = await DisablingAsync(
					scopeContext,
					job,
					uow,
					_serviceScope.ServiceProvider,
					cancellationToken);

				if (disabling)
					break;

				bool defaultHostIsAlive = true; //TODO
				bool currentHostIsAlive = true; //TODO
				var runResult = job.JobActivity.RunRunOnHost(scopeContext, _hostContext.IdHost, defaultHostIsAlive, currentHostIsAlive, _options.HostName);
				runResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				var saveResult = await uow.SaveAsync(scopeContext, cancellationToken: default);
				saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			}

			var jobExecutionContext = new JobExecutionContext();
			await ExecuteJob(scopeContext, jobExecutionContext, cancellationToken);

			if (jobExecutionContext.Terminate)
				break;

			await using (var uow = CreateUnitOfWork(_serviceScope.ServiceProvider))
			{
				var job = await uow.JobRepository
					.GetJobById(new Queries.Job.GetJobByIdQuery(JobContext.IdJob, false, false, true, null))
					.ToResultAsync(scopeContext, cancellationToken);

				if (job == null)
				{
					await WriteNoJobLog(scopeContext, uow, cancellationToken);
					break;
				}

				if (jobExecutionContext.ExecutedSuccessfully == true)
				{
					var runResult = job.JobActivity.FinishedSuccessfully(scopeContext, _options.HostName, jobExecutionContext.DelayedToUtc);
					runResult.ThrowIfErrorOrNullData(scopeContext, null, true);
				}
				else
				{
					var runResult = job.JobActivity.FinishedWithError(scopeContext, _options.HostName, jobExecutionContext.ErrorDetail, jobExecutionContext.DelayedToUtc);
					runResult.ThrowIfErrorOrNullData(scopeContext, null, true);
				}

				var saveResult = await uow.SaveAsync(scopeContext, cancellationToken: default);
				saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			}

			await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
		}

		cancellationToken = default; //original cancellationToken was canceled
		scopeContext = scopeContext.CreateNew();

		await using var uowCanceling = CreateUnitOfWork(_serviceScope.ServiceProvider);

		var cancelingJob = await uowCanceling.JobRepository
			.GetJobById(new Queries.Job.GetJobByIdQuery(JobContext.IdJob, false, false, true, null))
			.ToResultAsync(scopeContext, cancellationToken: default);

		if (cancelingJob == null)
		{
			await WriteNoJobLog(scopeContext, uowCanceling, cancellationToken: default);
			return;
		}

		var cancelingResult = cancelingJob.JobActivity.Canceling(scopeContext, _options.HostName);
		cancelingResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		var saveLogResult = await uowCanceling.SaveAsync(scopeContext, cancellationToken: default);
		saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);
	}

	public async Task ExecuteJob(
		IScopeContext scopeContext,
		JobExecutionContext jobExecutionContext,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName)
			.AddContextProperty(nameof(JobContext.JobName), JobContext.JobName);

		await using (var uow = CreateUnitOfWork(_serviceScope.ServiceProvider))
		{
			var job = await uow.JobRepository
				.GetJobById(new Queries.Job.GetJobByIdQuery(JobContext.IdJob, false, false, true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (job == null)
			{
				await WriteNoJobLog(scopeContext, uow, cancellationToken);
				jobExecutionContext.Terminate = true;
				return;
			}

			var executionStartTime = GlobalContext.Instance.UtcNow;
			var statistcsStartHour = executionStartTime.Date.AddHours(executionStartTime.Hour);

			var statistics = await uow.JobStatisticsRepository
				.GetJobStatisticsByJobIdAndStartHour(new Queries.JobStatistics.GetJobStatisticsByJobIdAndStartHourQuery(job.IdJob, statistcsStartHour, false, false, true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (statistics == null)
			{
				var createStatisticsResult = Model.JobStatistics.Create(scopeContext, job, statistcsStartHour);
				createStatisticsResult.ThrowIfErrorOrNullData(scopeContext, null, true);
				uow.JobStatisticsRepository.Add(scopeContext, createStatisticsResult.Data!);
			}
			else
			{
				var startResult = statistics.Start(scopeContext);
				startResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			}

			var createExecutionResult = Model.JobExecution.Create(scopeContext, job, executionStartTime, statistcsStartHour);
			createExecutionResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			uow.JobExecutionRepository.Add(scopeContext, createExecutionResult.Data!);

			try
			{
				//TODO Create JobRunningContext - nastav executionStartTime aj statistcsStartHour
				//vytvor instanciu z namespace
				//zavolaj metodu Execute

				Console.WriteLine($"Job {JobContext.JobName}: Doing work at {DateTime.Now:T}");

				jobExecutionContext.ExecutedSuccessfully = true;
				jobExecutionContext.DelayedToUtc = null;
				jobExecutionContext.ErrorDetail = null;
				jobExecutionContext.Terminate = false;
			}
			catch (Exception ex)
			{
				jobExecutionContext.ExecutedSuccessfully = false;
				jobExecutionContext.DelayedToUtc = null;
				jobExecutionContext.ErrorDetail = ex.ToString();
				jobExecutionContext.Terminate = false;
			}

			//TODO: update job activity
			//TODO: updated jobExecution
			//TODO: write result to log if needed

			var saveResult = await uow.SaveAsync(scopeContext, cancellationToken: default);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		}
	}

	public async ValueTask DisposeAsync()
	{
		if (_disposed)
			return;

		_disposed = true;

		await DisposeAsyncCoreAsync().ConfigureAwait(false);

		GC.SuppressFinalize(this);
	}

	protected virtual async ValueTask DisposeAsyncCoreAsync()
	{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, IdUnitOfWorkService.ToString());
#endif

		try
		{
			await StopAsync(ScopeContext.Create($"{nameof(JobService)} with job id {JobContext?.IdJob}"));
			_cancellationTokenSource.Dispose();
		}
		catch { }

		try
		{
			await _serviceScope.DisposeAsync();
		}
		catch { }
	}
}
