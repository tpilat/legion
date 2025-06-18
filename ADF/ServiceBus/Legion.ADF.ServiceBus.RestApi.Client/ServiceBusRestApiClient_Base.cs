using Legion.Http;
using Legion.Logging;
using Legion.NetHttp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public partial class ServiceBusRestApiClient : HttpApiClient<ServiceBusRestApiClientOptions>
{
	private const string CLIENT_NAME = nameof(ServiceBusRestApiClient);

	private static readonly Lazy<Newtonsoft.Json.JsonSerializerSettings> _jsonSerializerSettings = new(() =>
		new Newtonsoft.Json.JsonSerializerSettings
		{
		});

	public ServiceBusRestApiClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		IOptions<ServiceBusRestApiClientOptions> options,
		ILogger<HttpApiClient> logger)
		: base(client, serviceProvider, options, logger)
	{
	}

	protected IResult ToResult(IScopeContext scopeContext, IHttpClientRequest request, IHttpClientResponse response)
	{
		scopeContext = scopeContext.CreateNew();

		if (response.HasError(false))
		{
			var errorBuilder = LogError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.ErrorResponse(nameof(ServiceBusRestApiClient)), request, response);
			errorBuilder!.AppendDetail(CLIENT_NAME);

			return new ResultBuilder()
				.WithError(errorBuilder!.Build())
				.Build();
		}
		else //OK
		{
			return new ResultBuilder()
				.Build();
		}
	}

	protected async Task<IResult<TData>> ToJsonCollectionResultAsync<TData>(IScopeContext scopeContext, IHttpClientRequest request, IHttpClientResponse response, CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		if (response.HasError(true))
		{
			var errorBuilder = LogError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.ErrorResponse(nameof(ServiceBusRestApiClient)), request, response);
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

				var jsonObject = await response.ReadJsonContentAsAsync<TData>(_jsonSerializerSettings.Value, cancellationToken);
				if (jsonObject == null)
				{
					return new ResultBuilder<TData>()
						.WithError(errorBuilder!.Build())
						.Build();
				}
				else
				{
					return new ResultBuilder<TData>()
						.WithError(errorBuilder!.Build())
						.WithData(jsonObject)
						.Build();
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
				var jsonObject = await response.ReadJsonContentAsAsync<TData>(_jsonSerializerSettings.Value, cancellationToken);
				if (jsonObject == null)
					return new ResultBuilder<TData>()
						.WithError(LogMessage.CreateErrorMessage(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.InvalidJsonResponse(nameof(ServiceBusRestApiClient), typeof(TData).FullName!), x => x.InternalMessage($"{nameof(jsonObject)} == null").Detail(CLIENT_NAME)))
						.Build();

				return new ResultBuilder<TData>()
					.WithData(jsonObject)
					.Build();
			}
			catch (Exception ex)
			{
				return new ResultBuilder<TData>()
					.WithError(LogMessage.CreateErrorMessage(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex).Detail(CLIENT_NAME)))
					.Build();
			}
		}
	}

	protected async Task<IResult<TData>> ToJsonResultAsync<TData>(IScopeContext scopeContext, IHttpClientRequest request, IHttpClientResponse response, CancellationToken cancellationToken)
	{
		scopeContext = scopeContext.CreateNew();

		if (response.HasError(true))
		{
			var errorBuilder = LogError(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.ErrorResponse(nameof(ServiceBusRestApiClient)), request, response);
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

				var jsonObject = await response.ReadJsonContentAsAsync<TData>(_jsonSerializerSettings.Value, cancellationToken);
				if (jsonObject == null)
				{
					return new ResultBuilder<TData>()
						.WithError(errorBuilder!.Build())
						.Build();
				}
				else
				{
					return new ResultBuilder<TData>()
						.WithError(errorBuilder!.Build())
						.WithData(jsonObject)
						.Build();
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
				var jsonObject = await response.ReadJsonContentAsAsync<TData>(_jsonSerializerSettings.Value, cancellationToken);
				if (jsonObject == null)
					return new ResultBuilder<TData>()
						.WithError(LogMessage.CreateErrorMessage(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.InvalidJsonResponse(nameof(ServiceBusRestApiClient), typeof(TData).FullName!), x => x.InternalMessage($"{nameof(jsonObject)} == null").Detail(CLIENT_NAME)))
						.Build();

				return new ResultBuilder<TData>()
					.WithData(jsonObject)
					.Build();
			}
			catch (Exception ex)
			{
				return new ResultBuilder<TData>()
					.WithError(LogMessage.CreateErrorMessage(scopeContext, Legion.ADF.ServiceBus.RestApi.Client.Internal.ErrorCodes.ApiClientException.Default(nameof(ServiceBusRestApiClient)), x => x.ExceptionInfo(ex).Detail(CLIENT_NAME)))
					.Build();
			}
		}
	}
}
