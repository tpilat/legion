using Legion.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Http;

#if NET6_0_OR_GREATER
[Serializer.JsonPolymorphicConverter]
#endif
public interface IHttpClientResponse : IDisposable
{
	IHttpClientRequest Request { get; }
	HttpResponseMessage? HttpResponseMessage { get; }
	int? StatusCode { get; }
	bool? RequestTimedOut { get; }
	bool? OperationCanceled { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	Exception? Exception { get; }
	string? CancelOrTimeoutExceptionText { get; }
	string? ExceptionText { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	bool StatusCodeIsOK { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	bool IsOK { get; }

	ErrorMessageBuilder? GetErrorMessageBuilder(IScopeContext scopeContext, IErrorCode errorCode, bool checkResponseNotNull);

	Action<ErrorMessageBuilder>? GetErrorMessageBuilderAction(bool checkResponseNotNull);

	bool HasError(bool checkResponseNotNull);

	bool HasError(IScopeContext scopeContext, IErrorCode errorCode, [MaybeNullWhen(false)] out ErrorMessageBuilder errorMessageBuilder);

	bool HasErrorOrNoResponse(IScopeContext scopeContext, IErrorCode errorCode, [MaybeNullWhen(false)] out ErrorMessageBuilder errorMessageBuilder);

	bool HasError([MaybeNullWhen(false)] out Action<ErrorMessageBuilder> errorMessageBuilder);

	bool HasErrorOrNoResponse([MaybeNullWhen(false)] out Action<ErrorMessageBuilder> errorMessageBuilder);

	List<KeyValuePair<string, IEnumerable<string>>>? GetAllHeaders();
	List<KeyValuePair<string, IEnumerable<string>>>? GetResponseHeaders();
	List<KeyValuePair<string, IEnumerable<string>>>? GetContentHeaders();

	Task CopyContentToAsync(Stream stream, CancellationToken cancellationToken);
	Task<Stream?> ReadContentAsStreamAsync(CancellationToken cancellationToken);
	Task<byte[]?> ReadContentAsByteArrayAsync(CancellationToken cancellationToken);
	Task<string?> ReadContentAsStringAsync(CancellationToken cancellationToken);
	Task<T?> ReadJsonContentAsAsync<T>(Newtonsoft.Json.JsonSerializerSettings? jsonSerializerOptions = null, CancellationToken cancellationToken = default);
}
