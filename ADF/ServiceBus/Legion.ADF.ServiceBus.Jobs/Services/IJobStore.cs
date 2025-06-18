using Legion.Queries.Sorting;

namespace Legion.ADF.ServiceBus.Jobs.Services;

public interface  IJobStore : IDisposable, IAsyncDisposable
{
	Task<IResult<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>>>> GetJobsSummaryAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto> sortDescriptor,
		int pageIndex,
		int pageSize,
		CancellationToken cancellationToken = default);
}
