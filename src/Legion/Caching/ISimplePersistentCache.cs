namespace Legion.Caching;

public interface ISimplePersistentCache
{
	Task<bool> IsAliveAsync(CancellationToken cancellationToken = default);

	Task<(string? Value, Guid? RowVersion)> GetValueAsync(
		string key,
		CancellationToken cancellationToken = default);
	
	Task<bool> SetValuePermanentlyAsync(
		string key,
		string value,
		CancellationToken cancellationToken = default);

	Task<bool> SetValueWithSlidingExpirationAsync(
		string key,
		string value,
		TimeSpan slidingTime,
		CancellationToken cancellationToken = default);

	Task<bool> SetValueWithAbsoluteExpirationAsync(
		string key,
		string value,
		DateTime keepUntil,
		CancellationToken cancellationToken = default);

	Task<bool> SetValueWithAbsoluteServerSideExpirationAsync(
		string key,
		string value,
		TimeSpan deltaToNowUtc,
		CancellationToken cancellationToken = default);

	Task<bool> TryUpdateValuePermanentlyAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		CancellationToken cancellationToken = default);

	Task<bool> TryUpdateValueWithSlidingExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		TimeSpan slidingTime,
		CancellationToken cancellationToken = default);

	Task<bool> TryUpdateValueWithAbsoluteExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		DateTime keepUntil,
		CancellationToken cancellationToken = default);

	Task<bool> TryUpdateValueWithAbsoluteServerSideExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		Guid currentRowVersion,
		TimeSpan deltaToNowUtc,
		CancellationToken cancellationToken = default);

	Task<bool> RemoveValueAsync(
		string key,
		CancellationToken cancellationToken = default);
}
