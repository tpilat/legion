using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServiceBusRestApiClient(this IServiceCollection services,
		string? registerWithName = null,
		Action<IServiceProvider, HttpClient>? configureClient = null,
		Action<IHttpClientBuilder>? configureHttpClientBuilder = null)
	{
		services.AddHttpApiClient<ServiceBusRestApiClient, ServiceBusRestApiClientOptions, Guid?>(
			registerWithName, configureClient, configureHttpClientBuilder);

		return services;
	}
}
