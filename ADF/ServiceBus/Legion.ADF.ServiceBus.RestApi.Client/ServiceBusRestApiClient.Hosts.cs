using Legion.ADF.ServiceBus.DTOs.Hosts;
using Legion.ADF.ServiceBus.RestApi.Client.Requests;
using Legion.NetHttp;
using Legion.Queries.Sorting;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>
{
	public async Task<IResult<List<HostSummaryDto>>> GetHostsSummaryAsync(
		IScopeContext scopeContext,
		ISortDescriptorBuilder<HostSummaryDto> sortDescriptor,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Hosts.V1.GetHostsSummary);

		var result = new ResultBuilder<List<HostSummaryDto>>();

		if (result.IsNull(scopeContext, sortDescriptor))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageIndex, 0))
			return result.Build();

		if (result.IsLessThanOrEqual(scopeContext, pageSize, 0))
			return result.Build();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Hosts.V1.GetHostsSummary,
			timeoutInSeconds,
			queryString: null,
			new
			{
				SortDescriptor = sortDescriptor.Serialize(),
				PageIndex = pageIndex,
				PageSize = pageSize
			});

		try
		{
			using var response = await SendAsync(request, scopeContext, serviceProvider: null, cancellationToken: cancellationToken);
			var jsonResponse = await ToJsonResultAsync<List<HostSummaryDto>>(scopeContext, request, response, cancellationToken);
			return jsonResponse!;
		}
		catch (Exception ex)
		{
			return
				new ResultBuilder<List<HostSummaryDto>>()
					.WithError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex));
		}
	}

	public async Task<IResult<List<HostLogDto>>> GetHostLogsAsync(
		IScopeContext scopeContext,
		Guid idHost,
		DateTime from,
		DateTime to,
		int pageIndex,
		int pageSize,
		int? timeoutInSeconds = null,
		CancellationToken cancellationToken = default)
	{
		scopeContext = scopeContext.CreateNew()
			.AddContextProperty(nameof(Options.BaseAddress), Options?.BaseAddress)
			.AddContextProperty(nameof(URI), URI.Hosts.V1.GetHostLogs)
			.AddContextProperty(nameof(idHost), idHost.ToString())
			.AddContextProperty(nameof(from), from.ToString())
			.AddContextProperty(nameof(to), to.ToString());

		var result = new ResultBuilder<List<HostLogDto>>();

		var request = JsonRequestFactory.Create(
			Options!,
			Legion.Http.HttpMethod.Post,
			URI.Hosts.V1.GetHostLogs,
			timeoutInSeconds,
			queryString: null,
			new
			{
				IdHost = idHost,
				From = from,
				To = to,
				PageIndex = pageIndex,
				PageSize = pageSize
			});

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
}
