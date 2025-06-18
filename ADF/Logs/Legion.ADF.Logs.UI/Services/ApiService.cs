using System.Net.Http.Json;

namespace Legion.ADF.Logs.UI.Services;

public class ApiService
{
	private readonly HttpClient _httpClient;

	public ApiService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<T> GetAsync<T>(string url)
	{
		return await _httpClient.GetFromJsonAsync<T>(url);
	}
}

