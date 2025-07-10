using Legion.Caching;
using Legion.Http;
using Legion.Locks;
using Legion.Logging;
using Legion.NetHttp;
using Legion.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Cache.RestApi.Client;

public partial class CacheRestApiClient : HttpApiClient<CacheRestApiClientOptions>, ISimplePersistentCache, IDistributedLockProvider
{
	private const string CLIENT_NAME = nameof(CacheRestApiClient);

	private static readonly Lazy<Newtonsoft.Json.JsonSerializerSettings> _jsonSerializerSettings = new(() =>
		new Newtonsoft.Json.JsonSerializerSettings
		{
		});

	public CacheRestApiClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		IOptions<CacheRestApiClientOptions> options,
		ILogger<HttpApiClient> logger)
		: base(client, serviceProvider, options, logger)
	{
	}

	protected async Task<IResult<TData>> ToJsonResultAsync<TData>(IScopeContext scopeContext, IHttpClientRequest request, IHttpClientResponse response, CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		if (response.HasError(true))
		{
			var errorBuilder = LogError(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.ErrorResponse(nameof(CacheRestApiClient)), request, response);
			errorBuilder!.AppendDetail(CLIENT_NAME);
			try
			{
				if (response.HttpResponseMessage == null)
				{
					return new ResultBuilder<TData>()
						.WithError(errorBuilder!.Build())
						.Build();
				}
				else
				{
					response.HttpResponseMessage.Content.Headers.TryGetValues("Content-Length", out var contentLength);
					if (contentLength == null || !contentLength.Any() || contentLength?.FirstOrDefault() == "0")
					{
						return new ResultBuilder<TData>()
							.WithError(errorBuilder!.Build())
							.Build();
					}
				}

				var result = await response.ReadJsonContentAsAsync<ResultDto<TData>>(_jsonSerializerSettings.Value, cancellationToken);
				if (result == null)
				{
					return new ResultBuilder<TData>()
						.WithError(errorBuilder!.Build())
						.Build();
				}
				else
				{
					return result;
				}
			}
			catch
			{
				return new ResultBuilder<TData>()
					.WithError(errorBuilder!.Build())
					.Build();
			}
		}
		else //OK
		{
			try
			{
				var result = await response.ReadJsonContentAsAsync<ResultDto<TData>>(_jsonSerializerSettings.Value, cancellationToken);
				if (result == null)
					return new ResultBuilder<TData>()
						.WithError(LogMessage.CreateErrorMessage(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.InvalidJsonResponse(nameof(CacheRestApiClient), typeof(TData).FullName!), x => x.InternalMessage($"{nameof(result)} == null").Detail(CLIENT_NAME)))
						.Build();

				return result;
			}
			catch (Exception ex)
			{
				return new ResultBuilder<TData>()
					.WithError(LogMessage.CreateErrorMessage(scopeContext, Legion.ADF.Cache.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(CacheRestApiClient)), x => x.ExceptionInfo(ex).Detail(CLIENT_NAME)))
					.Build();
			}
		}
	}
}
