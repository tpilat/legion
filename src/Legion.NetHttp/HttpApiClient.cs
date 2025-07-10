using Legion.Extensions;
using Legion.Http;
using Legion.Logging;
using Legion.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text;

namespace Legion.NetHttp;

public abstract class HttpApiClient
{
	private readonly HttpClient _client;
	protected IServiceProvider ServiceProvider { get; }

	protected HttpApiClientOptions Options { get; }
	protected ILogger Logger { get; }

	public HttpApiClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		IOptions<HttpApiClientOptions> options,
		ILogger<HttpApiClient> logger)
	{
		_client = client ?? throw new ArgumentNullException(nameof(client));
		ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public HttpApiClient(
		HttpClient client,
		IServiceProvider serviceProvider,
		HttpApiClientOptions options,
		ILogger<HttpApiClient> logger)
	{
		_client = client ?? throw new ArgumentNullException(nameof(client));
		ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		Options = options ?? throw new ArgumentNullException(nameof(options));
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public Task<IHttpClientResponse> SendAsync(
		Action<RequestBuilder> configureRequest,
		IScopeContext scopeContext,
		IServiceProvider? serviceProvider = null,
		ITransactionsController? transactionsController = null,
		CancellationToken cancellationToken = default)
	{
		var builder = new RequestBuilder();
		configureRequest.Invoke(builder);

		return SendAsync(builder.Build(), scopeContext, serviceProvider, false, transactionsController, cancellationToken);
	}

	public Task<IHttpClientResponse> SendAsync(
		Action<RequestBuilder> configureRequest,
		IScopeContext scopeContext,
		IServiceProvider? serviceProvider,
		bool? continueOnCapturedContext,
		ITransactionsController? transactionsController = null,
		CancellationToken cancellationToken = default)
	{
		var builder = new RequestBuilder();
		configureRequest.Invoke(builder);

		return SendAsync(builder.Build(), scopeContext, serviceProvider, continueOnCapturedContext, transactionsController, cancellationToken);
	}

	public Task<IHttpClientResponse> SendAsync(
		HttpRequestMessage request,
		IScopeContext scopeContext,
		IServiceProvider serviceProvider,
		ITransactionsController? transactionsController = null,
		CancellationToken cancellationToken = default)
		=> SendAsync(HttpClientRequest.FromHttpRequest(request, false), scopeContext, serviceProvider, false, transactionsController, cancellationToken);

	public Task<IHttpClientResponse> SendAsync(
		IHttpClientRequest request,
		IScopeContext scopeContext,
		IServiceProvider? serviceProvider,
		ITransactionsController? transactionsController = null,
		CancellationToken cancellationToken = default)
		=> SendAsync(request, scopeContext, serviceProvider, false, transactionsController, cancellationToken);

	public async Task<IHttpClientResponse> SendAsync(
		IHttpClientRequest request,
		IScopeContext scopeContext,
		IServiceProvider? serviceProvider,
		bool? continueOnCapturedContext,
		ITransactionsController? transactionsController = null,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(request);
		Throw.IfArgumentNull(scopeContext);

		if (string.IsNullOrWhiteSpace(request.BaseAddress))
			request.BaseAddress = Options.BaseAddress;

		if (string.IsNullOrWhiteSpace(request.BaseAddress))
			request.BaseAddress = _client.BaseAddress?.ToString();

		if (!request.RequestTimeout.HasValue && Options.DefaultTimeoutInSeconds.HasValue)
			request.RequestTimeout = TimeSpan.FromSeconds(Options.DefaultTimeoutInSeconds.Value);

		Options.ConfigureStaticRequestParams(request);

		var response = new Http.Internal.HttpApiClientResponse(request);

		CancellationTokenSource? requestTimeoutCancellationTokenSource = null;
		CancellationTokenSource? linkedCancellationTokenSource = null;
		var usedCancellationToken = cancellationToken;
		try
		{
			if (request.RequestTimeout.HasValue)
			{
				requestTimeoutCancellationTokenSource = new CancellationTokenSource(request.RequestTimeout.Value);
				if (cancellationToken != default)
				{
					linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(requestTimeoutCancellationTokenSource.Token, cancellationToken);
					usedCancellationToken = linkedCancellationTokenSource.Token;
				}
				else
				{
					usedCancellationToken = requestTimeoutCancellationTokenSource.Token;
				}
			}

			using var httpRequestMessage = request.ToHttpRequestMessage();
#if NET6_0_OR_GREATER
			httpRequestMessage.Options.Set(HttpRequestOptionsKeys.ServiceProviderHttpRequestOptionsKey.Value, serviceProvider ?? ServiceProvider);
			httpRequestMessage.Options.Set(HttpRequestOptionsKeys.ScopeContextHttpRequestOptionsKey.Value, scopeContext);
			
			if (transactionsController != null)
				httpRequestMessage.Options.Set(HttpRequestOptionsKeys.TransactionsControllerHttpRequestOptionsKey.Value, transactionsController);

			if (0 < request.Items?.Count)
				httpRequestMessage.Options.Set(HttpRequestOptionsKeys.DictionaryItemsHttpRequestOptionsKey.Value, request.Items);
#elif NETSTANDARD2_0 || NETSTANDARD2_1
			httpRequestMessage.Properties.Add(HttpRequestOptionsKeys.ServiceProviderHttpRequestOptionsKeyName, serviceProvider ?? ServiceProvider);
			httpRequestMessage.Properties.Add(HttpRequestOptionsKeys.ScopeContextHttpRequestOptionsKeyName, scopeContext);

			if (transactionsController != null)
				httpRequestMessage.Properties.Add(HttpRequestOptionsKeys.TransactionsControllerHttpRequestOptionsKeyName, scopeContext);

			if (0 < request.Items?.Count)
				httpRequestMessage.Properties.Add(HttpRequestOptionsKeys.DictionaryItemsHttpRequestOptionsKeyName, request.Items);
#endif

			if (continueOnCapturedContext.HasValue)
			{
				var httpResponseMessageTask =
					_client
						.SendAsync(httpRequestMessage, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, usedCancellationToken)
						.ConfigureAwait(continueOnCapturedContext: continueOnCapturedContext.Value);

				response.HttpResponseMessage = await httpResponseMessageTask;
			}
			else
			{
				var httpResponseMessageTask =
					_client
						.SendAsync(httpRequestMessage, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, usedCancellationToken);

				response.HttpResponseMessage = await httpResponseMessageTask;
			}

			try
			{
#if NET6_0_OR_GREATER
				httpRequestMessage.Options.RemoveIfKeyExists(HttpRequestOptionsKeys.ServiceProviderHttpRequestOptionsKey.Value.Key);
				httpRequestMessage.Options.RemoveIfKeyExists(HttpRequestOptionsKeys.ScopeContextHttpRequestOptionsKey.Value.Key);
				httpRequestMessage.Options.RemoveIfKeyExists(HttpRequestOptionsKeys.TransactionsControllerHttpRequestOptionsKey.Value.Key);
				httpRequestMessage.Options.RemoveIfKeyExists(HttpRequestOptionsKeys.DictionaryItemsHttpRequestOptionsKey.Value.Key);
#elif NETSTANDARD2_0 || NETSTANDARD2_1
				httpRequestMessage.Properties.RemoveIfKeyExists(HttpRequestOptionsKeys.ServiceProviderHttpRequestOptionsKeyName);
				httpRequestMessage.Properties.RemoveIfKeyExists(HttpRequestOptionsKeys.ScopeContextHttpRequestOptionsKeyName);
				httpRequestMessage.Properties.RemoveIfKeyExists(HttpRequestOptionsKeys.TransactionsControllerHttpRequestOptionsKeyName);
				httpRequestMessage.Properties.RemoveIfKeyExists(HttpRequestOptionsKeys.DictionaryItemsHttpRequestOptionsKeyName);
#endif
			}
			catch { }
		}
		catch (TaskCanceledException)
		{
			if (requestTimeoutCancellationTokenSource != null && requestTimeoutCancellationTokenSource.IsCancellationRequested)
				response.RequestTimedOut = true;
			else
				response.OperationCanceled = true;
		}
		catch (TimeoutException)
		{
			response.RequestTimedOut = true;
		}
		catch (OperationCanceledException)
		{
			response.OperationCanceled = true;
		}
		catch (Exception ex)
		{
			response.Exception = ex;
		}
		finally
		{
			linkedCancellationTokenSource?.Dispose();
			requestTimeoutCancellationTokenSource?.Dispose();
		}

		return response;
	}

	protected virtual ErrorMessageBuilder? CreateErrorMessage(
		IScopeContext scopeContext,
		IErrorCode errorCode,
		IHttpClientRequest? request,
		IHttpClientResponse? response)
	{
		if (request == null && response == null)
			return null;

		scopeContext = ScopeContext.Create(scopeContext);
		ErrorMessageBuilder? builder;

		if (response == null)
		{
			builder = new ErrorMessageBuilder(scopeContext, errorCode);
			builder.InternalMessage("NO RESPONSE").Detail($"URI = {request?.GetRequestUri()}");
		}
		else
		{
			response.HasErrorOrNoResponse(scopeContext, errorCode, out builder);
		}

		return builder;
	}

	protected virtual ErrorMessageBuilder? LogError(
		IScopeContext scopeContext,
		IErrorCode errorCode,
		IHttpClientRequest? request,
		IHttpClientResponse? response)
	{
		if (request == null && response == null)
			return null;

		var errorMessageBuilder = CreateErrorMessage(scopeContext, errorCode, request, response);

		if (errorMessageBuilder == null)
			return null;

		Logger.LogErrorMessage(errorMessageBuilder.Build(), true);

		return errorMessageBuilder;
	}

	protected virtual StringBuilder LogErrorToStringBuilder(
		IHttpClientRequest? request,
		IHttpClientResponse? response,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string sourceFilePath = "",
		[CallerLineNumber] int sourceLineNumber = 0)
	{
		var sb = new StringBuilder();

		if (request != null)
			sb.AppendLine($"URI = {request.GetRequestUri()}");

		if (response != null)
		{
			if (request == null && response.Request != null)
				sb.AppendLine($"URI = {response.Request.GetRequestUri()}");

			sb.AppendLine($"{nameof(response.StatusCode)} = {response.StatusCode}");

			if (response.OperationCanceled.HasValue)
				sb.AppendLine($"{nameof(response.OperationCanceled)} = {response.OperationCanceled}");

			if (response.RequestTimedOut.HasValue)
				sb.AppendLine($"{nameof(response.RequestTimedOut)} = {response.RequestTimedOut}");

			if (response.Exception != null)
				sb.AppendLine($"Exception: {response.Exception.ToStringTrace()}");
		}
		else
		{
			sb.AppendLine("NO RESPONSE");
		}

		return sb;
	}
}
