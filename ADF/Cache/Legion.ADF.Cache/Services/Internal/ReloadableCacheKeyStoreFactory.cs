using Legion.ADF.Cache.Settings;
using Legion.Caching;
using Legion.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Cache.Services.Internal;

internal class ReloadableCacheKeyStoreFactory : IReloadableCacheKeyStoreFactory
{
	public IReloadableCacheKeyStore Create(IConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		var reloadableCacheKeyStoreOptions = connectionProvider.ServiceProvider.GetRequiredService<IOptions<ReloadableCacheKeyStoreOptions>>();
		var logger = connectionProvider.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<ReloadableCacheKeyStore>();

		return new ReloadableCacheKeyStore(connectionProvider, reloadableCacheKeyStoreOptions, logger);
	}
}
