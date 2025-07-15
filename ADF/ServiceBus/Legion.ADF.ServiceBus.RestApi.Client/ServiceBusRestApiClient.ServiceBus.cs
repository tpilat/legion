using Legion.ADF.ServiceBus.DTOs;
using Legion.ADF.ServiceBus.RestApi.Client.Requests;
using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>, IServiceBusMonitor
{
	public async Task<IResult<bool>> IsAliveV1Async(
		IScopeContext scopeContext,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.ServiceBus.V1.IsAlive);

		var result = new ResultBuilder<bool>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.ServiceBus.V1.IsAlive,
			timeoutInSeconds,
			queryString: null);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<bool>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<bool>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<ServiceBusInstancesDto>> GetServiceBusInstancesV1Async(
		IScopeContext scopeContext,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.ServiceBus.V1.GetInstances);

		var result = new ResultBuilder<ServiceBusInstancesDto>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.ServiceBus.V1.GetInstances,
			timeoutInSeconds,
			queryString: null);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<ServiceBusInstancesDto>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<ServiceBusInstancesDto>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	async Task<IResult<bool>> IServiceBusMonitor.IsAliveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<bool>();

		var res = await IsAliveV1Async(
			scopeContext,
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}

	async Task<IResult<ServiceBusInstancesDto>> IServiceBusMonitor.GetServiceBusInstancesAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<ServiceBusInstancesDto>();

		var res = await GetServiceBusInstancesV1Async(
			scopeContext,
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}
}
