using Legion.ADF.ServiceBus.DTOs.Hosts;
using Legion.ADF.ServiceBus.RestApi.Client.Requests;
using Legion.NetHttp;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>, IServiceBusMonitor
{
	public async Task<IResult<HostDetailDto>> GetHostDetailV1Async(
		IScopeContext scopeContext,
		DTOs.Hosts.GetHostDetailRequest req,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Host.V1.GetDetail);

		var result = new ResultBuilder<HostDetailDto>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Host.V1.GetDetail,
			timeoutInSeconds,
			queryString: null,
			req);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<HostDetailDto>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<HostDetailDto>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<HostLogDto>>> GetHostLogsV1Async(
		IScopeContext scopeContext,
		DTOs.Hosts.GetHostLogsRequest req,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Host.V1.GetLogs);

		var result = new ResultBuilder<List<HostLogDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Host.V1.GetLogs,
			timeoutInSeconds,
			queryString: null,
			req);

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<List<HostLogDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<HostLogDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	async Task<IResult<HostDetailDto>> IServiceBusMonitor.GetHostDetailAsync(
		IScopeContext scopeContext,
		Guid idHost,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<HostDetailDto>();

		var res = await GetHostDetailV1Async(
			scopeContext,
			new DTOs.Hosts.GetHostDetailRequest { IdHost = idHost },
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}

	async Task<IResult<List<HostLogDto>>> IServiceBusMonitor.GetHostLogsAsync(
		IScopeContext scopeContext,
		GetHostLogsRequest request,
		CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		var result = new ResultBuilder<List<HostLogDto>>();

		if (result.IsArgumentNull(scopeContext, request))
			return result.Build();

		var res = await GetHostLogsV1Async(
			scopeContext,
			request,
			timeoutInSeconds: null,
			cancellationToken).ConfigureAwait(false);

		result.MergeAllWithDataHasError(res);
		return result.Build();
	}
}
