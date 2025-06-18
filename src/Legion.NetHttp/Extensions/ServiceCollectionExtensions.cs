using Legion.NetHttp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Legion.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddHttpApiClient<TClient, TOptions, TCorrelation>(this IServiceCollection services,
		string? registerWithName = null,
		Action<IServiceProvider, HttpClient>? configureClient = null,
		Action<IHttpClientBuilder>? configureHttpClientBuilder = null)
		where TClient : HttpApiClient
		where TOptions : HttpApiClientOptions, new()
	{
		services.TryAddTransient<LogHandler<TOptions, TCorrelation>>();
		services.TryAddTransient<PolicyHandler<TOptions>>();

		var httpClientBuilder = string.IsNullOrWhiteSpace(registerWithName)
			? services.AddHttpClient<TClient>()
			: services.AddHttpClient<TClient>(registerWithName!);

		httpClientBuilder.ConfigureHttpClient((sp, httpClient) =>
		{
			TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Value;

			httpClient.DefaultRequestHeaders.Clear();

			if (!string.IsNullOrWhiteSpace(options.BaseAddress))
				httpClient.BaseAddress = new Uri(options.BaseAddress);

			if (!string.IsNullOrWhiteSpace(options.UserAgent))
				httpClient.DefaultRequestHeaders.Add("User-Agent", $"{options.UserAgent}{(options.Version == null ? "" : $" v{options.Version}")}");

			configureClient?.Invoke(sp, httpClient);
		});

		//na konci, aby to bol najviac outer handler
		httpClientBuilder
			.AddHttpMessageHandler<PolicyHandler<TOptions>>();

		configureHttpClientBuilder?.Invoke(httpClientBuilder);

		//na konci, aby to bol najviac inner handler
		httpClientBuilder
			.AddHttpMessageHandler<LogHandler<TOptions, TCorrelation>>();

		//if (options.ApplyToHttpClientHandler)
		//{
		httpClientBuilder
			.ConfigurePrimaryHttpMessageHandler(
				sp =>
				{
					TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Value;

					var handler = new System.Net.Http.HttpClientHandler();

					//if (options.ApplyToHttpClientHandler)
						options.ConfigureHttpClientHandler(handler);

					return handler;
			});
		//}

		return services;
	}
}
