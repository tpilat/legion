using Legion.Database;

namespace Legion.Caching;

public interface IReloadableCacheKeyStoreFactory
{
	IReloadableCacheKeyStore Create(IConnectionProvider connectionProvider);
}
