using Legion.ADF.ServiceBus.DTOs.Jobs;
using Legion.AspNetCore.WebApi;
using Legion.ExpressionsSerializer.SerializerHelpers;
using Legion.Queries.Sorting;
using Microsoft.AspNetCore.Mvc;

namespace Legion.ADF.ServiceBus.RestApi.Controllers.V1;

[ApiRoutePrefix("v1")]
[Route("[controller]")]
public class JobsController : ApiControllerBase
{
	[HttpPost, Route("GetJobsSummary")]
	public async Task<IResult<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>>>> GetJobsSummaryAsync(
		[FromBody] GetJobsSummaryRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();
		
		try
		{
			var jobsMonitor = GetRequiredService<IJobsMonitor>();

			var sortDescriptorBuilder =
				SortDescriptorSerializer.Deserialize(
					scopeContext,
					request.SortDescriptors,
					new SortDescriptorBuilder<Legion.ADF.ServiceBus.DTOs.Jobs.JobSummaryDto>()
						.SortBy(x => x.Name, System.ComponentModel.ListSortDirection.Ascending));

			var getResult = await jobsMonitor.GetJobsSummaryAsync(
				scopeContext,
				sortDescriptorBuilder,
				request.PageIndex,
				request.PageSize,
				cancellationToken);

			result.MergeAllWithDataHasError(getResult);

			return result.Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("GetJobsStatistics")]
	public async Task<IResult<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobStatisticsDto>>>> GetJobsStatisticsAsync(
		[FromBody] GetJobsStatisticsRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobStatisticsDto>>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var jobsMonitor = GetRequiredService<IJobsMonitor>();

			var getResult = await jobsMonitor.GetJobsStatisticsAsync(
				scopeContext,
				request.From,
				request.To,
				request.JobExecutionType,
				cancellationToken);

			result.MergeAllWithDataHasError(getResult);

			return result.Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("GetJobExecutions")]
	public async Task<IResult<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobExecutionDto>>>> GetJobExecutionsAsync(
		[FromBody] GetJobExecutionsRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobExecutionDto>>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var jobsMonitor = GetRequiredService<IJobsMonitor>();

			var getResult = await jobsMonitor.GetJobExecutionsAsync(
				scopeContext,
				request.IdJob,
				request.From,
				request.To,
				request.PageIndex,
				request.PageSize,
				cancellationToken);

			result.MergeAllWithDataHasError(getResult);

			return result.Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}

	[HttpPost, Route("GetJobLogs")]
	public async Task<IResult<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobLogDto>>>> GetJobLogsAsync(
		[FromBody] GetJobLogsRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<QueryResult<List<Legion.ADF.ServiceBus.DTOs.Jobs.JobLogDto>>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			var jobsMonitor = GetRequiredService<IJobsMonitor>();

			var getResult = await jobsMonitor.GetJobLogsAsync(
				scopeContext,
				request.IdJob,
				request.From,
				request.To,
				request.PageIndex,
				request.PageSize,
				cancellationToken);

			result.MergeAllWithDataHasError(getResult);

			return result.Build().ToDto();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex).ToDto();
		}
	}
}
