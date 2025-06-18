using Legion;
using Legion.Http;
using Legion.Logging;
using Legion.NetHttp;
using Legion.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestEnterpriseServiceBus.Exceptions.Internal;

namespace TestEnterpriseServiceBus.Adapters.SocPoist.Http;

public partial class SocPoistHttpClient : HttpApiClient
{
	private const string CLIENT_NAME = nameof(SocPoistHttpClient);

	private static readonly Lazy<Newtonsoft.Json.JsonSerializerSettings> _jsonSerializerSettings = new(() => new Newtonsoft.Json.JsonSerializerSettings()
	//TODO FIX .AddServiceReadConverters()
	);

	public SocPoistHttpClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		IOptions<SocPoistHttpClientOptions> options,
		ILogger<HttpApiClient> logger)
		: base(client, serviceProvider, options, logger)
	{
	}

	public SocPoistHttpClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		SocPoistHttpClientOptions options,
		ILogger<HttpApiClient> logger)
		: base(client, serviceProvider, options, logger)
	{
	}

	protected IResult ToResult(IScopeContext scopeContext, IHttpClientRequest request, IHttpClientResponse response)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (response.HasError(false))
		{
			var errorBuilder = LogError(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, request, response);
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
		scopeContext = ScopeContext.Create(scopeContext);

		if (response.HasError(true))
		{
			var errorBuilder = LogError(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, request, response);
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
						.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.InternalMessage($"{nameof(jsonObject)} == null").Detail(CLIENT_NAME)))
						.Build();

				return new ResultBuilder<TData>()
					.WithData(jsonObject)
					.Build();
			}
			catch (Exception ex)
			{
				return new ResultBuilder<TData>()
					.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.ExceptionInfo(ex).Detail(CLIENT_NAME)))
					.Build();
			}
		}
	}

	protected async Task<IResult<TData>> ToJsonResultAsync<TData>(IScopeContext scopeContext, IHttpClientRequest request, IHttpClientResponse response, CancellationToken cancellationToken)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (response.HasError(true))
		{
			var errorBuilder = LogError(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, request, response);
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
						.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.InternalMessage($"{nameof(jsonObject)} == null").Detail(CLIENT_NAME)))
						.Build();

				return new ResultBuilder<TData>()
					.WithData(jsonObject)
					.Build();
			}
			catch (Exception ex)
			{
				return new ResultBuilder<TData>()
					.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.ExceptionInfo(ex).Detail(CLIENT_NAME)))
					.Build();
			}
		}
	}

	protected async Task<IResult<string>> ToStringResultAsync(IScopeContext scopeContext, IHttpClientRequest request, IHttpClientResponse response, CancellationToken cancellationToken)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		if (response.HasError(true))
		{
			var errorBuilder = LogError(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, request, response);
			errorBuilder!.AppendDetail(CLIENT_NAME);
			try
			{
				if (response.HttpResponseMessage == null)
				{
					return new ResultBuilder<string>()
						.WithError(errorBuilder!.Build())
						.Build();
				}
				else
				{
					response.HttpResponseMessage.Content.Headers.TryGetValues("Content-Length", out var contentLength);
					if (contentLength == null || !contentLength.Any() || contentLength?.FirstOrDefault() == "0")
					{
						return new ResultBuilder<string>()
							.WithError(errorBuilder!.Build())
							.Build();
					}
				}

				var result = await response.ReadContentAsStringAsync(cancellationToken);

				return new ResultBuilder<string>()
					.WithError(errorBuilder!.Build())
					.WithData(result)
					.Build();
			}
			catch
			{
				return new ResultBuilder<string>()
					.WithError(errorBuilder!.Build())
					.Build();
			}
		}
		else //OK
		{
			try
			{
				var result = await response.ReadContentAsStringAsync(cancellationToken);
				if (result == null)
					return new ResultBuilder<string>()
						.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.InternalMessage($"{nameof(result)} == null").Detail(CLIENT_NAME)))
						.Build();

				return new ResultBuilder<string>()
					.WithData(result)
					.Build();
			}
			catch (Exception ex)
			{
				return new ResultBuilder<string>()
					.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.ExceptionInfo(ex).Detail(CLIENT_NAME)))
					.Build();
			}
		}
	}

	protected async Task<IResult<FormFile>> ToStreamResultAsync(IScopeContext scopeContext, Stream responseStream, IHttpClientRequest request, IHttpClientResponse response, CancellationToken cancellationToken)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		var contentType = response.GetContentHeaders()?.LastOrDefault(x => x.Key.Equals("Content-Type", StringComparison.InvariantCultureIgnoreCase)).Value?.FirstOrDefault();

		if (response.HasError(true))
		{
			var errorBuilder = LogError(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, request, response);
			errorBuilder!.AppendDetail(CLIENT_NAME);
			try
			{
				if (response.HttpResponseMessage == null)
				{
					return new ResultBuilder<FormFile>()
						.WithData(new FormFile { Content = responseStream, ContentType = contentType })
						.WithError(errorBuilder!.Build())
						.Build();
				}
				else
				{
					response.HttpResponseMessage.Content.Headers.TryGetValues("Content-Length", out var contentLength);
					if (contentLength == null || !contentLength.Any() || contentLength?.FirstOrDefault() == "0")
					{
						return new ResultBuilder<FormFile>()
							.WithData(new FormFile { Content = responseStream, ContentType = contentType })
							.WithError(errorBuilder!.Build())
							.Build();
					}
				}

				var content = await response.ReadContentAsStreamAsync(cancellationToken);

				if (content != null)
					await content.CopyToAsync(responseStream, cancellationToken);

				return new ResultBuilder<FormFile>()
					.WithError(errorBuilder!.Build())
					.WithData(new FormFile { Content = responseStream, ContentType = contentType })
					.Build();
			}
			catch
			{
				return new ResultBuilder<FormFile>()
					.WithData(new FormFile { Content = responseStream, ContentType = contentType })
					.WithError(errorBuilder!.Build())
					.Build();
			}
		}
		else //OK
		{
			try
			{
				var content = await response.ReadContentAsStreamAsync(cancellationToken);
				if (content == null)
					return new ResultBuilder<FormFile>()
						.WithData(new FormFile { Content = responseStream, ContentType = contentType })
						.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.InternalMessage($"{nameof(content)} == null").Detail(CLIENT_NAME)))
						.Build();
				else
					await content.CopyToAsync(responseStream, cancellationToken);

				return new ResultBuilder<FormFile>()
					.WithData(new FormFile { Content = responseStream, ContentType = contentType })
					.Build();
			}
			catch (Exception ex)
			{
				return new ResultBuilder<FormFile>()
					.WithData(new FormFile { Content = responseStream, ContentType = contentType })
					.WithError(LogMessage.CreateErrorMessage(scopeContext, ErrorCodes.SocPoistHttpClientException.Default, x => x.ExceptionInfo(ex).Detail(CLIENT_NAME)))
					.Build();
			}
		}
	}
}
