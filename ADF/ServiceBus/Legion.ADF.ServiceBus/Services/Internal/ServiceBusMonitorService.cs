using Legion.ADF.ServiceBus.Services.Internal.Dto;
using Legion.ADF.ServiceBus.Settings;
using Legion.Database;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ServiceBus.Services.Internal;

internal class ServiceBusMonitorService : UnitOfWorkServiceBase<IServiceBusUnitOfWork, ConnectionStringProvider>, IServiceBusMonitor, IDisposable, IAsyncDisposable
{
	private readonly ServiceBusMonitorOptions _options;

	public ServiceBusMonitorService(
		IOptions<ServiceBusMonitorOptions> options,
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

	public async Task<IResult<bool>> IsAliveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<bool>();

		try
		{
			var isALive = await UoW.HostRepository.IsAliveAsync(scopeContext, cancellationToken);

			return result
				.WithData(isALive)
				.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<DTOs.ServiceBusInstancesDto>> GetServiceBusInstancesAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DTOs.ServiceBusInstancesDto>();

		try
		{
			var serviceBusInstances = new ServiceBusInstances
			{
				Hosts = [],
				Jobs = []
			};

			await using var hostsUow = CreateStandaloneUnitOfWorkWithoutTransaction(ServiceProvider);
			await using var jobsUow = CreateStandaloneUnitOfWorkWithoutTransaction(ServiceProvider);

			var getAllHostsTask = hostsUow.HostRepository
				.GetAllHosts(new Queries.Host.GetAllHostsQuery(CheckReadPermissions: false, AsNoTracking: true, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			var getAllJobsTask = jobsUow.JobRepository
				.GetAllJobs(new Queries.Job.GetAllJobsQuery(CheckReadPermissions: false, AsNoTracking: true, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			try
			{
				await Task.WhenAll(getAllHostsTask, getAllJobsTask);

				serviceBusInstances.Hosts = (await getAllHostsTask) ?? [];
				serviceBusInstances.Jobs = (await getAllJobsTask) ?? [];
			}
			catch
			{
				if (getAllHostsTask.IsFaulted)
					throw getAllHostsTask.Exception;

				if (getAllJobsTask.IsFaulted)
					throw getAllJobsTask.Exception;
			}

			var serviceBusInstancesDto = new DTOs.ServiceBusInstancesDto
			{
				Hosts = serviceBusInstances.Hosts
					.Select(h => h.ToDto(scopeContext, _logger))
					.ToList(),
				Jobs = serviceBusInstances.Jobs
					.Select(h => h.ToDto())
					.ToList(),
			};

			serviceBusInstancesDto.IsDistributedManagerAvailable =
				await CacheManager.IsAliveDistributedCacheAsync(
					scopeContext,
					GetHostDistributedCacheKey("ALIVE"),
					ServiceProvider,
					ex =>
					{
						_logger.LogErrorMessage(
							scopeContext,
							Exceptions.Internal.ErrorCodes.ServiceBusMonitorException.DistributedManagerError,
							x => x.ExceptionInfo(ex));
					},
					cancellationToken);

			return result
				.WithData(serviceBusInstancesDto)
				.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<DTOs.Hosts.HostDetailDto>> GetHostDetailAsync(
		IScopeContext scopeContext,
		Guid idHost,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DTOs.Hosts.HostDetailDto>();

		try
		{
			var host = await UoW.HostRepository
				.GetHostById(new Queries.Host.GetHostByIdQuery(idHost, CheckReadPermissions: false, AsNoTracking: true, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			return result
				.WithData(host?.ToDetailDto(scopeContext, _logger))
				.Build();

		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<DTOs.Hosts.HostLogDto>>> GetHostLogsAsync(
		IScopeContext scopeContext,
		DTOs.Hosts.GetHostLogsRequest request,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<DTOs.Hosts.HostLogDto>>();

		try
		{
			if (result.IsArgumentNull(scopeContext, request))
				return result.Build();

			var hostLogs = await UoW.HostLogRepository
				.GetHostLogsByIdHost(new Queries.HostLog.GetHostLogsByIdHostQuery(
					request.IdHost,
					request.FromUtc,
					request.ToUtc,
					request.PageIndex,
					request.PageSize,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					null))
				.ToResultAsync(scopeContext, cancellationToken);

			return result
				.WithData(hostLogs.Select(x => x.ToDto()).ToList())
				.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<DTOs.Jobs.JobDetailDto>> GetJobDetailAsync(
		IScopeContext scopeContext,
		Guid idJob,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<DTOs.Jobs.JobDetailDto>();

		try
		{
			var job = await UoW.JobRepository
				.GetJobById(new Queries.Job.GetJobByIdQuery(idJob, CheckReadPermissions: false, AsNoTracking: true, DisableCahce: true, null))
				.ToResultAsync(scopeContext, cancellationToken);

			return result
				.WithData(job?.ToDetailDto())
				.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<DTOs.Jobs.JobStatisticsDto>>> GetJobStatisticsAsync(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobStatisticsRequest request,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<DTOs.Jobs.JobStatisticsDto>>();

		try
		{
			if (result.IsArgumentNull(scopeContext, request))
				return result.Build();

			var jobStatistics = await UoW.JobStatisticsRepository
				.GetJobStatisticsByJobId(new Queries.JobStatistics.GetJobStatisticsByJobIdQuery(
					request.IdJob,
					request.FromUtc,
					request.ToUtc,
					request.JobExecutionType,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					null))
				.ToResultAsync(scopeContext, cancellationToken);

			return result
				.WithData(jobStatistics.Select(x => x.ToDto()).ToList())
				.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<DTOs.Jobs.JobExecutionDto>>> GetJobExecutionsAsync(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobExecutionsRequest request,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<DTOs.Jobs.JobExecutionDto>>();

		try
		{
			if (result.IsArgumentNull(scopeContext, request))
				return result.Build();

			var jobExecutions = await UoW.JobExecutionRepository
				.GetJobExecutionById(new Queries.JobExecution.GetJobExecutionByIdQuery(
					request.IdJob,
					request.FromUtc,
					request.ToUtc,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					null))
				.ToResultAsync(scopeContext, cancellationToken);

			return result
				.WithData(jobExecutions.Select(x => x.ToDto()).ToList())
				.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<DTOs.Jobs.JobLogDto>>> GetJobLogsAsync(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobLogsRequest request,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<DTOs.Jobs.JobLogDto>>();

		try
		{
			if (result.IsArgumentNull(scopeContext, request))
				return result.Build();

			var jobLogs = await UoW.JobLogRepository
				.GetJobLogsByIdJob(new Queries.JobLog.GetJobLogsByIdJobQuery(
					request.IdJob,
					request.IdJobExecution,
					request.FromUtc,
					request.ToUtc,
					request.PageIndex,
					request.PageSize,
					CheckReadPermissions: false,
					AsNoTracking: true,
					DisableCahce: true,
					null))
				.ToResultAsync(scopeContext, cancellationToken);

			return result
				.WithData(jobLogs.Select(x => x.ToDto()).ToList())
				.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, x => x.ExceptionInfo(ex));
		}
	}

	private string GetHostDistributedCacheKey(
		string? operation)
		=> string.IsNullOrEmpty(operation)
			? $"{_options.MonitorIdentifier}:Legion.ADF.ServiceBus.Monitor"
			: $"{_options.MonitorIdentifier}:Legion.ADF.ServiceBus.Monitor:{operation}";
}
