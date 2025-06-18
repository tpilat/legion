namespace Legion.ADF.Cache;

public partial interface ICacheQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Cache.Model.Repositories.IVwReloadableCacheKeyRepository VwReloadableCacheKeyRepository { get; }
}
