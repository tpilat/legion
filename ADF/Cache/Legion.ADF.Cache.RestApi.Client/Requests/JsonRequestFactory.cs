using Legion.Http;

namespace Legion.ADF.Cache.RestApi.Client.Requests;

internal static class JsonRequestFactory
{
	public static IHttpClientRequest Create(
		CacheRestApiClientOptions options,
		Legion.Http.HttpMethod httpMethod,
		string relativePath,
		int? timeoutInSeconds,
		Dictionary<string, string>? queryString)
		=> new RequestBuilder()
			.BaseAddress(options?.BaseAddress)
			.RelativePath(relativePath)
			.Method(httpMethod.ToHttpMethod())
			.RequestTimeout(timeoutInSeconds.HasValue ? TimeSpan.FromSeconds(timeoutInSeconds.Value) : null)
			//.ConfigureHeaders(x => x.CustomHeaders.Add(new ForceableKeyValuePair { Key = "X-API-KEY", Value = options?.ApiKey! }))
			.QueryString(queryString)
			.Build();

	public static IHttpClientRequest Create<T>(
		CacheRestApiClientOptions options,
		Legion.Http.HttpMethod httpMethod,
		string relativePath,
		int? timeoutInSeconds,
		Dictionary<string, string>? queryString,
		T @object,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings = null)
		=> new RequestBuilder()
			.BaseAddress(options?.BaseAddress)
			.RelativePath(relativePath)
			.Method(httpMethod.ToHttpMethod())
			.RequestTimeout(timeoutInSeconds.HasValue ? TimeSpan.FromSeconds(timeoutInSeconds.Value) : null)
			//.ConfigureHeaders(x => x.CustomHeaders.Add(new ForceableKeyValuePair { Key = "X-API-KEY", Value = options?.ApiKey! }))
			.QueryString(queryString)
			.AddJsonContent(
				new JsonContent<T>
				{
					Content = @object,
					MediaType = null,
					JsonSerializerSettings = jsonSerializerSettings ?? JsonSerializer.JsonSerializerOptions
				})
			.Build();
}
