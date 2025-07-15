namespace Legion.ADF.ServiceBus;

public interface IServiceBusMonitor : IDisposable, IAsyncDisposable
{
	Task<IResult<bool>> IsAliveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Task<IResult<DTOs.ServiceBusInstancesDto>> GetServiceBusInstancesAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Task<IResult<DTOs.Hosts.HostDetailDto>> GetHostDetailAsync(
		IScopeContext scopeContext,
		Guid idHost,
		CancellationToken cancellationToken = default);

	Task<IResult<List<DTOs.Hosts.HostLogDto>>> GetHostLogsAsync(
		IScopeContext scopeContext,
		DTOs.Hosts.GetHostLogsRequest request,
		CancellationToken cancellationToken = default);

	Task<IResult<DTOs.Jobs.JobDetailDto>> GetJobDetailAsync(
		IScopeContext scopeContext,
		Guid idJob,
		CancellationToken cancellationToken = default);

	Task<IResult<List<DTOs.Jobs.JobStatisticsDto>>> GetJobStatisticsAsync(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobStatisticsRequest request,
		CancellationToken cancellationToken = default);

	Task<IResult<List<DTOs.Jobs.JobExecutionDto>>> GetJobExecutionsAsync(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobExecutionsRequest request,
		CancellationToken cancellationToken = default);

	Task<IResult<List<DTOs.Jobs.JobLogDto>>> GetJobLogsAsync(
		IScopeContext scopeContext,
		DTOs.Jobs.GetJobLogsRequest request,
		CancellationToken cancellationToken = default);
}
