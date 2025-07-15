using Legion.AspNetCore.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace Legion.ADF.ServiceBus.RestApi.Controllers.V1;

[ApiRoutePrefix("v1")]
[Route("[controller]")]
public class ServiceBusController : ApiControllerBase
{
	[HttpPost, Route("IsAlive")]
	public async Task<IResult<bool>> IsAliveAsync(
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<bool>();

		await using var monitorService = GetRequiredService<IServiceBusMonitor>();

		var isAliveResult = await monitorService.IsAliveAsync(
			scopeContext,
			cancellationToken);

		result.MergeAllWithDataHasError(isAliveResult);

		return result
			.Build()
			.ToDto();
	}

	[HttpPost, Route("GetInstances")]
	public async Task<IResult<DTOs.ServiceBusInstancesDto>> GetInstancesAsync(
		CancellationToken cancellationToken = default)
	{
		var scopeContext = GetNewScopeContext();

		var result = new ResultBuilder<DTOs.ServiceBusInstancesDto>();
				
		try
		{
			await using var monitorService = GetRequiredService<IServiceBusMonitor>();

			var serviceBusInstancesResult = await monitorService.GetServiceBusInstancesAsync(
				scopeContext,
				cancellationToken);

			result.MergeAllWithDataHasError(serviceBusInstancesResult);

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
