using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServiceBusRestApiClient(this IServiceCollection services,
		string configBindingPath,
		string? registerWithName = null,
		Action<IServiceProvider, HttpClient>? configureClient = null,
		Action<IHttpClientBuilder>? configureHttpClientBuilder = null)
	{
		services.AddHttpApiClient<ServiceBusRestApiClient, ServiceBusRestApiClientOptions, Guid?>(
			registerWithName, configureClient, configureHttpClientBuilder);

		Assembly[] assemblies = [
			typeof(ServiceBusRestApiClient).Assembly
		];

		services.AddValidators(ServiceLifetime.Singleton, assemblies);
		services.AddServiceBusRestApiClientOptions(configBindingPath);

		return services;
	}
}
