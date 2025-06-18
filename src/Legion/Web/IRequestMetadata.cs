namespace Legion.Web;

#if NET6_0_OR_GREATER
[Legion.Serializer.JsonPolymorphicConverter]
#endif
public interface IRequestMetadata
{
	IReadOnlyList<KeyValuePair<string, List<string>>>? Query { get; }
	string? ContentType { get; }
	long? ContentLength { get; }
	IReadOnlyList<KeyValuePair<string, string>>? Cookies { get; }
	IReadOnlyDictionary<string, List<string>>? Headers { get; }
	string? Protocol { get; }
	IReadOnlyDictionary<string, object?>? RouteValues { get; }
	string? Path { get; }
	string? PathBase { get; }
	string? Host { get; }
	string? RemoteIp { get; }
	int? Port { get; }
	Uri? Uri { get; }
	string? Scheme { get; }
	string? Method { get; }
	IReadOnlyList<KeyValuePair<string, List<string>>>? Form { get; }
	int? FilesCount { get; }
	IReadOnlyDictionary<string, Func<string, string>>? CookieUnprotectors { get; } //Dictionary<cookieName, Unprotector>

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IServiceProvider? RequestServiceProvider { get; }

	string UnprotectCookie(string key, string value);
}
