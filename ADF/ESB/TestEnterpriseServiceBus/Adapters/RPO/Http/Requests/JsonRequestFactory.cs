using Legion.Http;
using Legion.NetHttp;
using System.Net.Http.Headers;

namespace TestEnterpriseServiceBus.Adapters.RPO.Http.Requests;

internal static class JsonRequestFactory
{
	public static IHttpClientRequest Create(
		HttpApiClientOptions Options,
		Legion.Http.HttpMethod httpMethod,
		string relativePath,
		Dictionary<string, string>? queryString,
		Guid idEsbAdapter)
		=> new RequestBuilder()
			.BaseAddress(Options?.BaseAddress)
			.RelativePath(relativePath)
			.Method(httpMethod.ToHttpMethod())
			.RequestTimeout(Options?.DefaultTimeoutInSeconds.HasValue == true ? TimeSpan.FromSeconds(Options.DefaultTimeoutInSeconds.Value) : null)
			.QueryString(queryString)
			.AddItem(Legion.ADF.ESB.Components.AdapterRequestResponseLogger.PARAM_idAdapter, idEsbAdapter)
			.Build();

	public static IHttpClientRequest Create<T>(
		HttpApiClientOptions Options,
		Legion.Http.HttpMethod httpMethod,
		string relativePath,
		Dictionary<string, string>? queryString,
		Guid idEsbAdapter,
		T @object,
		Newtonsoft.Json.JsonSerializerSettings? options = null)
		=> new RequestBuilder()
			.BaseAddress(Options?.BaseAddress)
			.RelativePath(relativePath)
			.Method(httpMethod.ToHttpMethod())
			.RequestTimeout(Options?.DefaultTimeoutInSeconds.HasValue == true ? TimeSpan.FromSeconds(Options.DefaultTimeoutInSeconds.Value) : null)
			.QueryString(queryString)
			.AddJsonContent(
				new JsonContent<T>
				{
					Content = @object,
					MediaType = MediaTypeHeaderValue.Parse("application/json"),
					JsonSerializerSettings = options ?? JsonSerializer.JsonSerializerSettings
				})
			.AddItem(Legion.ADF.ESB.Components.AdapterRequestResponseLogger.PARAM_idAdapter, idEsbAdapter)
			.Build();
}
