using Microsoft.Extensions.Caching.Memory;

namespace Legion.ADF.Cache.Model.Repositories;

public partial interface ICacheDataRepository : Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.CacheData>
{
	Task<bool> IsAliveAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Task<Cache.Model.CacheData?> TryGetCacheDataAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default);

	Task<string?> TryGetValueAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default);

	Task<bool> SetAsync(
		IScopeContext scopeContext,
		string key,
		string value,
		Guid? currentRowVersion,
		MemoryCacheEntryOptions? options,
		CancellationToken cancellationToken = default);

	Task<bool> TryUpdateAsync(
		IScopeContext scopeContext,
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		MemoryCacheEntryOptions? options = null,
		CancellationToken cancellationToken = default);

	Task<bool> RemoveAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default);

	Task DeleteExpiredAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
