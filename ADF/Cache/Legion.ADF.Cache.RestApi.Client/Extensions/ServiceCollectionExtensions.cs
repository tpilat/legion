using Legion.Caching;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Cache.RestApi.Client;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCacheRestApiClient(this IServiceCollection services,
		string configBindingPath,
		string? registerWithName = null,
		Action<IServiceProvider, HttpClient>? configureClient = null,
		Action<IHttpClientBuilder>? configureHttpClientBuilder = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(configBindingPath);

		services.AddHttpApiClient<CacheRestApiClient, CacheRestApiClientOptions, Guid?>(
			registerWithName, configureClient, configureHttpClientBuilder);

		Assembly[] assemblies = [
			typeof(CacheRestApiClient).Assembly
		];

		services.AddValidators(ServiceLifetime.Singleton, assemblies);
		services.AddCacheRestApiClientOptions(configBindingPath);
		services.TryAddTransient<ISimplePersistentCache, CacheRestApiClient>();

		return services;
	}
}
