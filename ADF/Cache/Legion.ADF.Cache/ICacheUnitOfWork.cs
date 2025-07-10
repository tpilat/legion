using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Cache;

public partial interface ICacheUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Cache.Model.Repositories.ICacheDataRepository CacheDataRepository { get; }

	Legion.ADF.Cache.Model.Repositories.IDistributedLockRepository DistributedLockRepository { get; }

	Legion.ADF.Cache.Model.Repositories.IReloadableCacheKeyRepository ReloadableCacheKeyRepository { get; }
}
