using Legion.ADF.Cache.Services;
using Legion.ADF.Cache.Services.Internal;
using Legion.ADF.Cache.Settings;
using Legion.Caching;
using Legion.Extensions;
using Legion.Locks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Legion.ADF.Cache.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFCacheBuilder AddADFCache(
		this IServiceCollection services,
		IConfiguration? configuration = null)
	{

		//settings / options
		services.AddAppSettings();

		Assembly[] assemblies = [
			typeof(ADFCacheBuilder).Assembly,
			typeof(ADFCache).Assembly
		];

		//Add all validators from Legion.ADF.Cache.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		services.AddSingleton<IMemoryCache>(sp =>
		{
			var adfCacheOptions = sp.GetRequiredService<IOptions<ADFCacheOptions>>().Value;

			var memoryCacheOptions = new MemoryCacheOptions
			{
				SizeLimit = adfCacheOptions.SizeLimit
			};

			return new MemoryCache(memoryCacheOptions);
		});

		services.TryAddSingleton<IReloadableCacheKeyStoreFactory, ReloadableCacheKeyStoreFactory>();
		services.TryAddSingleton<IADFCache, ADFCache>();
		services.TryAddSingleton<IPersistentCache, PersistentCache>();
		services.TryAddSingleton<ISimplePersistentCache, PersistentCache>();
		services.TryAddTransient<ReloadableCacheKeyStore>();
		services.TryAddTransient<IReloadableCacheKeyStore, ReloadableCacheKeyStore>();
		services.TryAddSingleton<IDistributedLockProvider, DistributedLockProvider>();

		services.AddHostedService<CacheDataRemoveService>();
		services.AddHostedService<CacheKeyRemoveService>();

		return new ADFCacheBuilder(services, configuration);
	}
}
