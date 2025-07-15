using Legion.AspNetCore.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace Legion.ADF.ServiceBus.RestApi.Controllers.V1;

[ApiRoutePrefix("v1")]
[Route("[controller]")]
public class HostController : ApiControllerBase
{
	[HttpPost, Route("GetDetail")]
	public async Task<IResult<DTOs.Hosts.HostDetailDto>> GetDetailAsync(
		[FromBody] DTOs.Hosts.GetHostDetailRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<DTOs.Hosts.HostDetailDto>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			await using var monitorService = GetRequiredService<IServiceBusMonitor>();

			var hostDetailResult = await monitorService.GetHostDetailAsync(
				scopeContext,
				request.IdHost,
				cancellationToken);

			result.MergeAllWithDataHasError(hostDetailResult);

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
	public async Task<IResult<List<DTOs.Hosts.HostLogDto>>> GetLogsAsync(
		[FromBody] DTOs.Hosts.GetHostLogsRequest request,
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<List<DTOs.Hosts.HostLogDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build().ToDto();

		try
		{
			await using var monitorService = GetRequiredService<IServiceBusMonitor>();

			var hostLogsResult = await monitorService.GetHostLogsAsync(
				scopeContext,
				request,
				cancellationToken);

			result.MergeAllWithDataHasError(hostLogsResult);

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
