#if NET6_0_OR_GREATER
using Legion.Extensions;
using Legion.Logging;
using Legion.Serializer;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Http.Internal;

public class HttpApiClientResponse : IHttpClientResponse, IDisposable
{
	private bool disposedValue;

	public IHttpClientRequest Request { get; }
	public HttpResponseMessage? HttpResponseMessage { get; set; }

	public int? StatusCode => (int?)HttpResponseMessage?.StatusCode;

	public bool? RequestTimedOut { get; set; }

	public bool? OperationCanceled { get; set; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public Exception? Exception { get; set; }

	public string? CancelOrTimeoutExceptionText =>
		OperationCanceled == true
			? "Operation was cancelled"
			: (RequestTimedOut == true
				? "Request timed out"
				: null);

	public string? ExceptionText => Exception?.ToStringTrace() ?? CancelOrTimeoutExceptionText;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public bool StatusCodeIsOK => StatusCode.HasValue && StatusCode.Value < 400;

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	public bool IsOK =>
		StatusCodeIsOK
		&& Exception == null
		&& OperationCanceled != true
		&& RequestTimedOut != true;

	public HttpApiClientResponse(IHttpClientRequest request)
	{
		Request = request ?? throw new ArgumentNullException(nameof(request));
	}

	public Task CopyContentToAsync(Stream stream, CancellationToken cancellationToken)
	{
		if (HttpResponseMessage?.Content != null)
			return HttpResponseMessage.Content.CopyToAsync(stream, cancellationToken);

		return Task.CompletedTask;
	}

	public List<KeyValuePair<string, IEnumerable<string>>>? GetAllHeaders()
	{
		var responseHeaders = GetResponseHeaders();

		if (responseHeaders == null)
		{
			return GetContentHeaders();
		}
		else
		{
			var contentHeader = GetContentHeaders();
			if (contentHeader != null)
				responseHeaders.AddRange(contentHeader);

			return responseHeaders;
		}
	}

	public List<KeyValuePair<string, IEnumerable<string>>>? GetResponseHeaders()
		=> HttpResponseMessage?.Headers?.ToList();

	public List<KeyValuePair<string, IEnumerable<string>>>? GetContentHeaders()
		=> HttpResponseMessage?.Content.Headers?.ToList();

	public Task<Stream?> ReadContentAsStreamAsync(CancellationToken cancellationToken)
		=> HttpResponseMessage?.Content == null
			? Task.FromResult((Stream?)null)
			: HttpResponseMessage.Content.ReadAsStreamAsync(cancellationToken) as Task<Stream?>;

	public Task<byte[]?> ReadContentAsByteArrayAsync(CancellationToken cancellationToken)
		=> HttpResponseMessage?.Content == null
			? Task.FromResult((byte[]?)null)
			: HttpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken) as Task<byte[]?>;

	public Task<string?> ReadContentAsStringAsync(CancellationToken cancellationToken)
		=> HttpResponseMessage?.Content == null
			? Task.FromResult((string?)null)
			: HttpResponseMessage.Content.ReadAsStringAsync(cancellationToken) as Task<string?>;

	public async Task<T?> ReadJsonContentAsAsync<T>(
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings = null, 
		CancellationToken cancellationToken = default)
	{
		if (HttpResponseMessage == null)
			return default;

		using var stream = await HttpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
		var result = await JsonSerializerHelper.DeserializeAsync<T>(stream, jsonSerializerSettings, cancellationToken).ConfigureAwait(false);
		return result;
	}

	public bool HasError(bool checkResponseNotNull)
	{
		return Exception != null
			|| OperationCanceled == true
			|| RequestTimedOut == true
			|| !StatusCodeIsOK
			|| checkResponseNotNull && HttpResponseMessage == null;
	}

	public Action<ErrorMessageBuilder>? GetErrorMessageBuilderAction(bool checkResponseNotNull)
	{
		var cancelOrTimeoutText = CancelOrTimeoutExceptionText;
		if (Exception != null)
		{
			var builderAction =
				(Action<ErrorMessageBuilder>)(x => x
					.ExceptionInfo(Exception)
					.Detail(Request.GetRequestUri())
					.AppendDetail(StatusCode == null ? null : $"{nameof(StatusCode)} = {StatusCode}")
					.AppendDetail(cancelOrTimeoutText));

			if (checkResponseNotNull && HttpResponseMessage == null)
				builderAction += x => x.AppendDetail($"{nameof(HttpResponseMessage)} == null");

			return builderAction;
		}
		else if (!string.IsNullOrWhiteSpace(cancelOrTimeoutText))
		{
			var builderAction =
				(Action<ErrorMessageBuilder>)(x => x
					.InternalMessage(cancelOrTimeoutText)
					.Detail(Request.GetRequestUri())
					.AppendDetail(StatusCode == null ? null : $"{nameof(StatusCode)} = {StatusCode}"));

			if (checkResponseNotNull && HttpResponseMessage == null)
				builderAction += x => x.AppendDetail($"{nameof(HttpResponseMessage)} == null");

			return builderAction;
		}
		else if (!StatusCodeIsOK)
		{
			var builderAction =
				(Action<ErrorMessageBuilder>)(x => x
					.InternalMessage($"{nameof(StatusCode)} = {StatusCode}")
					.Detail(Request.GetRequestUri()));

			if (checkResponseNotNull && HttpResponseMessage == null)
				builderAction += x => x.AppendDetail($"{nameof(HttpResponseMessage)} == null");

			return builderAction;
		}
		else if (checkResponseNotNull && HttpResponseMessage == null)
		{
			void builderAction(ErrorMessageBuilder x) => x
					.InternalMessage($"{nameof(HttpResponseMessage)} == null")
					.Detail(Request.GetRequestUri())
					.AppendDetail(StatusCode == null ? null : $"{nameof(StatusCode)} = {StatusCode}");

			return builderAction;
		}

		return null;
	}

	public ErrorMessageBuilder? GetErrorMessageBuilder(IScopeContext scopeContext, IErrorCode errorCode, bool checkResponseNotNull)
	{
		var action = GetErrorMessageBuilderAction(checkResponseNotNull);
		if (action != null)
		{
			var builder = new ErrorMessageBuilder(scopeContext, errorCode);
			action?.Invoke(builder);
			return builder;
		}

		return null;
	}

	public bool HasError(IScopeContext scopeContext, IErrorCode errorCode, [MaybeNullWhen(false)] out ErrorMessageBuilder errorMessageBuilder)
	{
		errorMessageBuilder = GetErrorMessageBuilder(scopeContext, errorCode, false);
		return errorMessageBuilder != null;
	}

	public bool HasErrorOrNoResponse(IScopeContext scopeContext, IErrorCode errorCode, [MaybeNullWhen(false)] out ErrorMessageBuilder errorMessageBuilder)
	{
		errorMessageBuilder = GetErrorMessageBuilder(scopeContext, errorCode, true);
		return errorMessageBuilder != null;
	}

	public bool HasError([MaybeNullWhen(false)] out Action<ErrorMessageBuilder> errorMessageBuilder)
	{
		errorMessageBuilder = GetErrorMessageBuilderAction(false);
		return errorMessageBuilder != null;
	}

	public bool HasErrorOrNoResponse([MaybeNullWhen(false)] out Action<ErrorMessageBuilder> errorMessageBuilder)
	{
		errorMessageBuilder = GetErrorMessageBuilderAction(true);
		return errorMessageBuilder != null;
	}

	public override string ToString()
	{
		return JsonSerializerHelper.Serialize(this, new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented });
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing && HttpResponseMessage != null)
			{
				HttpResponseMessage.Dispose();
			}

			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
#endif
