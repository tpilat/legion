using Legion.AspNetCore.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace Legion.ADF.ServiceBus.RestApi.Controllers.V1;

[ApiRoutePrefix("v1")]
[Route("[controller]")]
public class JobController : ApiControllerBase
{
	[HttpPost, Route("GetDetail")]
	public async Task<IResult<DTOs.Jobs.JobDetailDto>> GetDetailAsync(
		[FromBody] DTOs.Jobs.GetJobDetailRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<DTOs.Jobs.JobDetailDto>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			await using var monitor = GetRequiredService<IServiceBusMonitor>();

			var jobDetailResult = await monitor.GetJobDetailAsync(
				scopeContext,
				request.IdJob,
				cancellationToken);

			result.MergeAllWithDataHasError(jobDetailResult);

			return result
				.Build()
				.ToDto();
		}
		catch (Exception ex)
		{
			return HttpContext.RequestServices.GetRequiredService<ILogger<ServiceBusController>>()
				.LogResultErrorMessages(
					result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex),
					skipIfAlreadyLogged: true,
					logWarnings: true)
				.ToDto();
		}
	}

	[HttpPost, Route("GetStatistics")]
	public async Task<IResult<List<DTOs.Jobs.JobStatisticsDto>>> GetStatisticsAsync(
		[FromBody] DTOs.Jobs.GetJobStatisticsRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<List<DTOs.Jobs.JobStatisticsDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			await using var monitor = GetRequiredService<IServiceBusMonitor>();

			var jobStatisticsResult = await monitor.GetJobStatisticsAsync(
				scopeContext,
				request,
				cancellationToken);

			result.MergeAllWithDataHasError(jobStatisticsResult);

			return result
				.Build()
				.ToDto();
		}
		catch (Exception ex)
		{
			return HttpContext.RequestServices.GetRequiredService<ILogger<ServiceBusController>>()
				.LogResultErrorMessages(
					result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex),
					skipIfAlreadyLogged: true,
					logWarnings: true)
				.ToDto();
		}
	}

	[HttpPost, Route("GetExecutions")]
	public async Task<IResult<List<DTOs.Jobs.JobExecutionDto>>> GetExecutionsAsync(
		[FromBody] DTOs.Jobs.GetJobExecutionsRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<List<DTOs.Jobs.JobExecutionDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			await using var monitor = GetRequiredService<IServiceBusMonitor>();

			var jobExecutionsResult = await monitor.GetJobExecutionsAsync(
				scopeContext,
				request,
				cancellationToken);

			result.MergeAllWithDataHasError(jobExecutionsResult);

			return result
				.Build()
				.ToDto();
		}
		catch (Exception ex)
		{
			return HttpContext.RequestServices.GetRequiredService<ILogger<ServiceBusController>>()
				.LogResultErrorMessages(
					result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex),
					skipIfAlreadyLogged: true,
					logWarnings: true)
				.ToDto();
		}
	}

	[HttpPost, Route("GetLogs")]
	public async Task<IResult<List<DTOs.Jobs.JobLogDto>>> GetLogsAsync(
		[FromBody] DTOs.Jobs.GetJobLogsRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<List<DTOs.Jobs.JobLogDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			await using var monitor = GetRequiredService<IServiceBusMonitor>();

			var jobLogsResult = await monitor.GetJobLogsAsync(
				scopeContext,
				request,
				cancellationToken);

			result.MergeAllWithDataHasError(jobLogsResult);

			return result
				.Build()
				.ToDto();
		}
		catch (Exception ex)
		{
			return HttpContext.RequestServices.GetRequiredService<ILogger<ServiceBusController>>()
				.LogResultErrorMessages(
					result.WithInvalidOperationException(scopeContext, errorCode: null, detail: null, ex),
					skipIfAlreadyLogged: true,
					logWarnings: true)
				.ToDto();
		}
	}
}
