using Legion.Clones;

namespace Legion.Caching;

public interface IADFCache : IDisposable
{
	T? GetValue<T>(string key, bool getClone = true, ICloneFactory? cloneFactory = null);
	
	IEnumerable<string> GetAllKeys();
	
	bool SetValuePermanently<T>(
		string key,
		T value,
		List<string>? tags = null,
		bool setNullValue = false,
		bool createClone = true,
		ICloneFactory? cloneFactory = null);

	bool SetValueWithSlidingExpiration<T>(
		string key,
		T value,
		TimeSpan slidingTime,
		List<string>? tags = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		bool setNullValue = false,
		bool createClone = true,
		ICloneFactory? cloneFactory = null);

	bool SetValueWithAbsoluteExpiration<T>(
		string key,
		T value,
		DateTime keepUntil,
		List<string>? tags = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		bool setNullValue = false,
		bool createClone = true,
		ICloneFactory? cloneFactory = null);

	T GetOrSetValuePermanently<T>(
		string key,
		Func<T> value,
		List<string>? tags = null,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null);

	T GetOrSetValueWithSlidingExpiration<T>(
		string key,
		Func<T> value,
		TimeSpan slidingTime,
		List<string>? tags = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null);

	T GetOrSetValueWithAbsoluteExpiration<T>(
		string key,
		Func<T> value,
		DateTime keepUntil,
		List<string>? tags = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null);

	Task<T> GetOrSetValuePermanentlyAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> value,
		List<string>? tags = null,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null,
		CancellationToken cancellationToken = default);

	Task<T> GetOrSetValueWithSlidingExpirationAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> value,
		TimeSpan slidingTime,
		List<string>? tags = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null,
		CancellationToken cancellationToken = default);

	Task<T> GetOrSetValueWithAbsoluteExpirationAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> value,
		DateTime keepUntil,
		List<string>? tags = null,
		CacheItemPriority priority = CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null,
		CancellationToken cancellationToken = default);

	void RemoveValue(string key);
	
	void RemoveValuesForTag(string tag);

	void RemoveValuesForWholeTags(List<string> tags);
}
