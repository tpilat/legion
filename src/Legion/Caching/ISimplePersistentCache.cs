namespace Legion.Caching;

public interface ISimplePersistentCache
{
	Task<(string? Value, long? RowVersion)> GetValue(
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

	Task<bool> TryUpdateValuePermanentlyAsync(
		string key,
		string oldValue,
		string newValue,
		long currentRowVersion,
		CancellationToken cancellationToken = default);

	Task<bool> TryUpdateValueWithSlidingExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		long currentRowVersion,
		TimeSpan slidingTime,
		CancellationToken cancellationToken = default);

	Task<bool> TryUpdateValueWithAbsoluteExpirationAsync(
		string key,
		string oldValue,
		string newValue,
		long currentRowVersion,
		DateTime keepUntil,
		CancellationToken cancellationToken = default);

	Task<bool> RemoveValueAsync(
		string key,
		CancellationToken cancellationToken = default);
}
