namespace Legion.Caching;

public interface IPersistentCache : ISimplePersistentCache, IDisposable
{
	Task<string> GetOrSetValuePermanentlyAsync(
		string key,
		Func<string> value,
		bool forceSet = false,
		CancellationToken cancellationToken = default);

	Task<string> GetOrSetValueWithSlidingExpirationAsync(
		string key,
		Func<string> value,
		TimeSpan slidingTime,
		bool forceSet = false,
		CancellationToken cancellationToken = default);

	Task<string> GetOrSetValueWithAbsoluteExpirationAsync(
		string key,
		Func<string> value,
		DateTime keepUntil,
		bool forceSet = false,
		CancellationToken cancellationToken = default);

	Task<string> GetOrSetValuePermanentlyAsync(
		string key,
		Func<CancellationToken, Task<string>> value,
		bool forceSet = false,
		CancellationToken cancellationToken = default);

	Task<string> GetOrSetValueWithSlidingExpirationAsync(
		string key,
		Func<CancellationToken, Task<string>> value,
		TimeSpan slidingTime,
		bool forceSet = false,
		CancellationToken cancellationToken = default);

	Task<string> GetOrSetValueWithAbsoluteExpirationAsync(
		string key,
		Func<CancellationToken, Task<string>> value,
		DateTime keepUntil,
		bool forceSet = false,
		CancellationToken cancellationToken = default);
}
