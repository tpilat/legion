namespace Legion.ADF.Cache.Model.Repositories;

public partial interface IDistributedLockRepository : Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.DistributedLock>
{
	Task<Cache.Model.DistributedLock?> TryGetDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		CancellationToken cancellationToken = default);

	Task<DistributedLock?> TryAcquireDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		TimeSpan timeout,
		string? metadata,
		TimeSpan? retryDelay = null,
		int? maxRetries = null,
		CancellationToken cancellationToken = default);

	Task<string?> TryAcquireDistributedLockIdAsync(
		IScopeContext scopeContext,
		string key,
		TimeSpan timeout,
		string? metadata,
		TimeSpan? retryDelay = null,
		int? maxRetries = null,
		CancellationToken cancellationToken = default);

	Task<bool> ReleaseDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		string lockId,
		CancellationToken cancellationToken = default);

	Task<bool> RenewDistributedLockAsync(
		IScopeContext scopeContext,
		string key,
		string lockId,
		TimeSpan timeout,
		CancellationToken cancellationToken = default);

	Task DeleteExpiredAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
