namespace Legion.Locks;

public interface IDistributedLockProvider
{
	Task<bool> ExistsAsync(
		string key,
		CancellationToken cancellationToken = default);

	Task<string?> GetMetadataAsync(
		string key,
		CancellationToken cancellationToken = default);

	Task<string?> TryAcquireLockAsync(
		string key,
		TimeSpan timeout,
		string? metadata,
		TimeSpan? retryDelay = null,
		int? maxRetries = null,
		CancellationToken cancellationToken = default);

	Task<bool> ReleaseLockAsync(
		string key,
		string lockId,
		CancellationToken cancellationToken = default);

	Task<bool> RenewLockAsync(
		string key,
		string lockId,
		TimeSpan timeout,
		CancellationToken cancellationToken = default);
}
