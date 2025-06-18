using Legion.Http.Headers;

namespace Legion.Http;

#if NET6_0_OR_GREATER
[Serializer.JsonPolymorphicConverter]
#endif
public interface IHttpClientRequest
{
	string? BaseAddress { get; set; }
	string? RelativePath { get; set; }
	string? QueryString { get; set; }
	string? HttpMethod { get; set; }
	bool ClearDefaultHeaders { get; set; }
	RequestHeaders Headers { get; }
	string? MultipartSubType { get; set; }
	string? MultipartBoundary { get; set; }
	TimeSpan? RequestTimeout { get; set; }
	List<KeyValuePair<string, string>>? FormData { get; set; }
	List<StringContent>? StringContents { get; set; }
	List<JsonContent>? JsonContents { get; set; }
	List<StreamContent>? StreamContents { get; set; }
	List<ByteArrayContent>? ByteArrayContents { get; set; }

	Dictionary<string, object?> Items { get; set; }

	HttpRequestMessage ToHttpRequestMessage();
	string? GetRequestUri();
	System.Net.Http.HttpContent? ToHttpContent();
}
