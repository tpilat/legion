using Legion.ADF.ServiceBus.Services.Internal.Dto;
using Legion.ADF.ServiceBus.Settings;
using Legion.Caching;
using Legion.Database;
using Legion.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ServiceBus.Services.Internal;

internal partial class HostService : UnitOfWorkServiceBase<IServiceBusUnitOfWork, ConnectionStringProvider>, IDisposable, IAsyncDisposable
{
	private readonly EnterpriseServiceBusOptions _options;

	public HostService(
		IOptions<EnterpriseServiceBusOptions> options,
		IServiceProvider serviceProvider,
		ILogger<HostService> logger)
		: base(
			ScopeContext.Create(nameof(HostService)),
			Throw.IfArgumentNull(options)?.Value.StoreId,
			Throw.IfArgumentNull(serviceProvider),
			Throw.IfArgumentNull(logger))
	{
		_options = options.Value;
	}

	private IResult CreateLogHost(
		IScopeContext scopeContext,
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
			UoW.HostLogRepository.Add(scopeContext, createdResult.Data!);

		return createdResult;
	}

	public async Task StartHostAsync(
		IScopeContext scopeContext,
		HostContext hostContext,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName);

		try
		{
			var host = await UoW.HostRepository
				.GetHostByName(new Queries.Host.GetHostByNameQuery(_options.HostName, GetDisabledHost: true, CheckReadPermissions: false, AsNoTracking: false, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (host == null)
			{
				_logger.LogErrorMessage(
					scopeContext,
					Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.NoHostFound(_options.HostName));

				await Task.Delay(TimeSpan.FromSeconds(_options.NoHostTimeoutInSeconds), cancellationToken);
				hostContext.IncrementError();
				return;
			}

			hostContext.SetIdHost(host.IdHost);

			if (host.HostActivity == null)
			{
				var createResult = Model.HostActivity.Create(scopeContext, host);
				createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				UoW.HostActivityRepository.Add(scopeContext, createResult.Data!);
			}

			var cfgResult = host.GetHostConfiguration(scopeContext);
			cfgResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			hostContext.SetHostConfiguration(cfgResult.Data!);

			var cfgValidationResult = DTOs.Hosts.HostConfigurationDto.DefaultValidator.Value.Validate(hostContext.HostConfiguration);
			if (cfgValidationResult.HasError)
			{
				CreateLogHost(
					scopeContext,
					host.IdHost,
					Codes.InvalidConfig,
					force: true,
					new LogMessageBuilder(scopeContext, errorCode: null)
						.LogLevel(LogLevel.Error)
						.InternalMessage($"Host {host.Name}::{host.IdHost} has invalid configuration")
						.Detail(cfgValidationResult.ToText(scopeContext, Environment.NewLine, withDetail: true, withSeverity: false))
						.Build(),
					isRunning: false)
					.ThrowIfErrorOrNullData(scopeContext, null, true);

				var saveLogResult = await UoW.SaveAsync(scopeContext, cancellationToken);
				saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				await Task.Delay(hostContext.GetErrorDelay(), cancellationToken);
				hostContext.IncrementError();
				return;
			}

			if (!host.IsEnabled)
			{
				CreateLogHost(
					scopeContext,
					host.IdHost,
					Codes.Disabled,
					force: true,
					new LogMessageBuilder(scopeContext, errorCode: null)
						.LogLevel(LogLevel.Warning)
						.InternalMessage($"Host {host.Name}::{host.IdHost} is disabled, cannot start")
						.Build(),
					isRunning: false)
					.ThrowIfErrorOrNullData(scopeContext, null, true);

				var saveLogResult = await UoW.SaveAsync(scopeContext, cancellationToken);
				saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				await Task.Delay(hostContext.GetErrorDelay(), cancellationToken);
				hostContext.IncrementError();
				return;
			}

			var updated = host.HostActivity!.SetStart(scopeContext);
			updated.ThrowIfError(scopeContext, null, true);

			CreateLogHost(
				scopeContext,
				host.IdHost,
				Codes.Started,
				force: true,
				new LogMessageBuilder(scopeContext, errorCode: null)
					.LogLevel(LogLevel.Information)
					.InternalMessage($"Host {host.Name}::{host.IdHost} is started.")
					.Build(),
				isRunning: true)
				.ThrowIfErrorOrNullData(scopeContext, null, true);

			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			hostContext.SetStarted(hostContext.HostConfiguration.HeartbeatInSeconds);
		}
		catch (Exception ex)
		{
			hostContext.IncrementError();

			_logger.LogCriticalMessage(
				scopeContext,
				Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, hostContext.IdHost),
				x => x.ExceptionInfo(ex, force: true));

			await Task.Delay(hostContext.GetErrorDelay(), cancellationToken);
		}
	}

	public async Task HostHeartbeatAsync(
		IScopeContext scopeContext,
		HostContext hostContext,
		IServiceProvider serviceProvider,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName);

		try
		{
			var host = await UoW.HostRepository
				.GetHostByName(new Queries.Host.GetHostByNameQuery(_options.HostName, GetDisabledHost: true, CheckReadPermissions: false, AsNoTracking: false, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			if (host?.HostActivity == null)
			{
				_logger.LogErrorMessage(
					scopeContext,
					Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.NoHostFound(_options.HostName));

				await Task.Delay(TimeSpan.FromSeconds(_options.NoHostTimeoutInSeconds), cancellationToken);
				hostContext.IncrementError();
				return;
			}

			hostContext.SetIdHost(host.IdHost);

			var cfgResult = host.GetHostConfiguration(scopeContext);
			cfgResult.ThrowIfErrorOrNullData(scopeContext, null, true);
			hostContext.SetHostConfiguration(cfgResult.Data!);

			var cfgValidationResult = DTOs.Hosts.HostConfigurationDto.DefaultValidator.Value.Validate(hostContext.HostConfiguration);
			if (cfgValidationResult.HasError)
			{
				CreateLogHost(
					scopeContext,
					host.IdHost,
					Codes.InvalidConfig,
					force: true,
					new LogMessageBuilder(scopeContext, errorCode: null)
						.LogLevel(LogLevel.Error)
						.InternalMessage($"Host {host.Name}::{host.IdHost} has invalid configuration")
						.Detail(cfgValidationResult.ToText(scopeContext, Environment.NewLine, withDetail: true, withSeverity: false))
						.Build(),
					isRunning: false)
					.ThrowIfErrorOrNullData(scopeContext, null, true);

				var saveLogResult = await UoW.SaveAsync(scopeContext, cancellationToken);
				saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				await Task.Delay(hostContext.GetErrorDelay(), cancellationToken);
				hostContext.IncrementError();
				return;
			}

			if (!host.IsEnabled)
			{
				CreateLogHost(
					scopeContext,
					host.IdHost,
					Codes.Disabled,
					force: true,
					new LogMessageBuilder(scopeContext, errorCode: null)
						.LogLevel(LogLevel.Warning)
						.InternalMessage($"Host {host.Name}::{host.IdHost} is disabled, cannot update last activity")
						.Build(),
					isRunning: false)
					.ThrowIfErrorOrNullData(scopeContext, null, true);

				var saveLogResult = await UoW.SaveAsync(scopeContext, cancellationToken);
				saveLogResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				await Task.Delay(hostContext.GetErrorDelay(), cancellationToken);
				hostContext.IncrementError();
				return;
			}

			var utcNow = GlobalContext.Instance.UtcNow;

			var isAvailableDistributedCache = await WriteDistributedHeartbeatAsync(
				scopeContext,
				host.HostActivity,
				utcNow,
				hostContext,
				serviceProvider,
				cancellationToken);

			var updated = host.HostActivity.UpdateLastActivity(scopeContext, utcNow, isAvailableDistributedCache);
			updated.ThrowIfError(scopeContext, null, true);

			if (!isAvailableDistributedCache)
			{
				//TODO: STOP ALL JOBS!!!!!!!!!!!!!!!!!!!!!! ???????????????????????????????????????   Naozaj to treba urobit?   ???????????????????????????

				var updateResult = await UoW.SaveAsync(scopeContext, cancellationToken);
				updateResult.ThrowIfErrorOrNullData(scopeContext, null, true);

				await Task.Delay(hostContext.GetErrorDelay(), cancellationToken);
				hostContext.IncrementError();
				return;
			}

			CreateLogHost(
				scopeContext,
				host.IdHost,
				Codes.Heartbeat,
				force: false,
				new LogMessageBuilder(scopeContext, errorCode: null)
					.LogLevel(LogLevel.Trace)
					.InternalMessage($"Host {host.Name}::{host.IdHost} heartbeat")
					.Build(),
				isRunning: true)
				.ThrowIfErrorOrNullData(scopeContext, null, true);

			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);

			await LoadServiceBusInstancesAsync(
				scopeContext,
				hostContext,
				serviceProvider,
				cancellationToken);

			await RunJobsAsync(
				scopeContext,
				hostContext,
				serviceProvider,
				cancellationToken);

			await Task.Delay(TimeSpan.FromSeconds(hostContext.HostConfiguration.HeartbeatInSeconds), cancellationToken);
			hostContext.SetHeartbeatSuccess();
		}
		catch (Exception ex)
		{
			hostContext.IncrementError();

			_logger.LogCriticalMessage(
				scopeContext,
				Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, hostContext.IdHost),
				x => x.ExceptionInfo(ex, force: true));

			await Task.Delay(hostContext.GetErrorDelay(), cancellationToken);
		}
	}

	private async Task<bool> WriteDistributedHeartbeatAsync(
		IScopeContext scopeContext,
		Model.HostActivity hostActivity,
		DateTime utcNow,
		HostContext hostContext,
		IServiceProvider serviceProvider,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		return await CacheManager.WriteDistributedHeartbeatAsync(
			scopeContext,
			_options.CacheKeySystemName,
			hostActivity,
			utcNow,
			hostContext,
			serviceProvider,
			ex =>
			{
				CreateLogHost(
					scopeContext,
					hostActivity.Host.IdHost,
					Codes.Heartbeat,
					force: false,
					new LogMessageBuilder(scopeContext, errorCode: null)
						.LogLevel(LogLevel.Error)
						.ExceptionInfo(ex)
						.Build(),
					isRunning: false)
				.ThrowIfErrorOrNullData(scopeContext, null, true);
			},
			cancellationToken);
	}

	private async Task LoadServiceBusInstancesAsync(
		IScopeContext scopeContext,
		HostContext hostContext,
		IServiceProvider? serviceProvider,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName);

		var serviceBusInstances = new ServiceBusInstances
		{
			Hosts = [],
			Jobs = []
		};

		await using var hostsUow = CreateStandaloneUnitOfWorkWithoutTransaction(serviceProvider);
		await using var jobsUow = CreateStandaloneUnitOfWorkWithoutTransaction(serviceProvider);

		var getAllHostsTask = hostsUow.HostRepository
			.GetAllHosts(new Queries.Host.GetAllHostsQuery(CheckReadPermissions: false, AsNoTracking: true, DisableCahce: true, null))
			.ToResultAsync(scopeContext, cancellationToken);

		var getAllJobsTask = jobsUow.JobRepository
			.GetAllJobs(new Queries.Job.GetAllJobsQuery(CheckReadPermissions: false, AsNoTracking: true, DisableCahce: true, null))
			.ToResultAsync(scopeContext, cancellationToken);

		try
		{
			await Task.WhenAll(getAllHostsTask, getAllJobsTask);

			serviceBusInstances.Hosts = await getAllHostsTask;
			serviceBusInstances.Jobs = await getAllJobsTask;

			foreach (var host in serviceBusInstances.Hosts)
			{
				host.DefaultJobs = [];
				host.RunningOwnJobs = [];
				host.RunningForeignJobs = [];
			}
		}
		catch
		{
			if (getAllHostsTask.IsFaulted)
				throw getAllHostsTask.Exception;

			if (getAllJobsTask.IsFaulted)
				throw getAllJobsTask.Exception;
		}

		serviceBusInstances.MyHost = serviceBusInstances.Hosts.FirstOrDefault(x => x.IdHost == hostContext.IdHost)!;
		if (serviceBusInstances.MyHost == null)
			Throw.IfNull(serviceBusInstances.MyHost, scopeContext);

		foreach (var job in serviceBusInstances.Jobs)
		{
			job.DefaultHost = serviceBusInstances.Hosts.FirstOrDefault(x => x.IdHost == job.IdDefaultHost);
			if (job.DefaultHost != null)
			{
				job.DefaultHost.DefaultJobs!.Add(job);
			}

			if (job.JobActivity != null)
			{
				job.CurrentHost = serviceBusInstances.Hosts.FirstOrDefault(x => x.IdHost == job.JobActivity.IdCurrentHost);
				if (job.CurrentHost != null)
				{
					if (job.IdDefaultHost == job.JobActivity.IdCurrentHost)
					{
						job.CurrentHost.RunningOwnJobs!.Add(job);
					}
					else
					{
						job.CurrentHost.RunningForeignJobs!.Add(job);
					}
				}
			}

			if (hostContext.RunningJobs.TryGetValue(job.IdJob, out var jobService))
				jobService.JobContext.UpdateJobInfo(job);
		}

		hostContext.SetServiceBusInstances(serviceBusInstances);
	}

	private async Task RunJobsAsync(
		IScopeContext scopeContext,
		HostContext hostContext,
		IServiceProvider serviceProvider,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName);

		Throw.IfArgumentNull(serviceProvider, scopeContext);

		var serviceBusInstances = hostContext.ServiceBusInstances?.Clone();

		if (serviceBusInstances?.Jobs == null || serviceBusInstances.Jobs.Count == 0)
			return;

		foreach (var job in serviceBusInstances.Jobs)
		{
			bool defaultHostIsAlive = true; //TODO
			bool currentHostIsAlive = true; //TODO
			if (job.CanRunOnHost(hostContext.IdHost, defaultHostIsAlive, currentHostIsAlive))
			{
				var serviceScope = serviceProvider.CreateAsyncScope();
				JobService? newJobService = null;
				hostContext.RunningJobs.GetOrAdd(job.IdJob, id =>
				{
					newJobService = new JobService(
						hostContext,
						new JobContext(job.IdJob, job.Name),
						_options,
						serviceScope);

					return newJobService;
				});

				if (newJobService != null)
				{
					var started = await newJobService.TryStartAsync(scopeContext, cancellationToken);
				}
			}
		}
	}

	public async Task StopHostAsync(
		IScopeContext scopeContext,
		HostContext hostContext
		/*CancellationToken cancellationToken -- cancellationToken WAS ALWAYS CANCELED*/)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(_options.HostName), _options.HostName);

		try
		{
			var host = await UoW.HostRepository
				.GetHostByName(new Queries.Host.GetHostByNameQuery(_options.HostName, GetDisabledHost: true, CheckReadPermissions: false, AsNoTracking: false, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken: default);

			if (host?.HostActivity == null)
				return;

			hostContext.SetIdHost(host.IdHost);

			var updated = host.HostActivity.SetStop(scopeContext);
			updated.ThrowIfError(scopeContext, null, true);

			CreateLogHost(
				scopeContext,
				host.IdHost,
				Codes.Stopped,
				force: true,
				new LogMessageBuilder(scopeContext, errorCode: null)
					.LogLevel(LogLevel.Information)
					.InternalMessage($"Host {host.Name}::{host.IdHost} is started.")
					.Build(),
				isRunning: true)
				.ThrowIfErrorOrNullData(scopeContext, null, true);

			var saveResult = await UoW.SaveAsync(scopeContext, cancellationToken: default);
			saveResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		}
		catch (Exception ex)
		{
			_logger.LogCriticalMessage(
				scopeContext,
				Legion.ADF.ServiceBus.Exceptions.Internal.ErrorCodes.ServiceBusHostException.UnhandledError(_options.HostName, hostContext.IdHost),
				x => x.ExceptionInfo(ex, force: true));
		}
	}
}
