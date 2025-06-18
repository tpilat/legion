using Legion.ADF.Cache.Settings;
using Legion.Caching;
using Legion.Clones;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Legion.ADF.Cache.Services;

public class ADFCache : IADFCache, IDisposable
{
	private readonly IMemoryCache _cache;
	private readonly CacheKeys _cacheKeys;
	private readonly ADFCacheOptions _options;
	private readonly ICloneFactory _defaultCloneFactory;

	private bool disposed;

	public ADFCache(IMemoryCache memoryCache, IOptions<ADFCacheOptions> options)
	{
		Throw.IfArgumentNull(memoryCache);
		Throw.IfArgumentNull(options);

		_cache = memoryCache;
		_options = options.Value;
		_cacheKeys = new CacheKeys();
		_defaultCloneFactory = _options.CloneFactory ?? new ReflectionCloneFactory(); //new JsonCloneFactory();
	}

	[return: NotNullIfNotNull(nameof(@object))]
	private T? Clone<T>(T? @object, bool clone, ICloneFactory? cloneFactory)
	{
		if (!clone || @object == null)
			return @object;

		var result = cloneFactory != null
			? cloneFactory.Clone(@object)
			: _defaultCloneFactory.Clone(@object);

		return result;
	}

	public T? GetValue<T>(string key, bool getClone = true, ICloneFactory? cloneFactory = null)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		_cache.TryGetValue(key, out T? value);

		value = Clone(value, getClone, cloneFactory);
		return value;
	}

	public IEnumerable<string> GetAllKeys()
		=> _cacheKeys.GetAllKeys();

	public bool SetValue<T>(
		string key,
		T value,
		List<string>? tags,
		MemoryCacheEntryOptions? options,
		bool setNullValue,
		bool createClone,
		ICloneFactory? cloneFactory)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNull(value);
		
		if (setNullValue || value != null)
		{
			value = Clone(value, createClone, cloneFactory);

			_cacheKeys.Add(key, tags);
			_cache.Set(key, value, (options ?? new()).RegisterPostEvictionCallback(_cacheKeys.RemoveCallback));
			return true;
		}

		return false;
	}

	public bool SetValuePermanently<T>(
		string key,
		T value,
		List<string>? tags = null,
		bool setNullValue = false,
		bool createClone = true,
		ICloneFactory? cloneFactory = null)
		=> SetValue(
			key,
			value,
			tags,
			options: null,
			setNullValue,
			createClone,
			cloneFactory);

	public bool SetValueWithSlidingExpiration<T>(
		string key,
		T value,
		TimeSpan slidingTime,
		List<string>? tags = null,
		Microsoft.Extensions.Caching.Memory.CacheItemPriority priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
		bool setNullValue = false,
		bool createClone = true,
		ICloneFactory? cloneFactory = null)
		=> SetValue(
			key,
			value,
			tags,
			new MemoryCacheEntryOptions
			{
				SlidingExpiration = slidingTime,
				Priority = priority
			},
			setNullValue,
			createClone,
			cloneFactory);

	public bool SetValueWithAbsoluteExpiration<T>(
		string key,
		T value,
		DateTime keepUntil,
		List<string>? tags = null,
		Microsoft.Extensions.Caching.Memory.CacheItemPriority priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
		bool setNullValue = false,
		bool createClone = true,
		ICloneFactory? cloneFactory = null)
		=> SetValue(
			key,
			value,
			tags,
			new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = keepUntil,
				Priority = priority
			},
			setNullValue,
			createClone,
			cloneFactory);

	public T GetOrSetValue<T>(
		string key,
		Func<T> value,
		List<string>? tags,
		MemoryCacheEntryOptions? options,
		bool forceSet,
		bool setNullValue,
		CacheCloneOption cloneOptions,
		ICloneFactory? cloneFactory)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNull(value);

		var cachedValue = GetValue<T>(key, cloneOptions.CloneGet(), cloneFactory);

		if (!forceSet && cachedValue != null)
			return cachedValue;

		var result = value();
		var resultClone = Clone(result, cloneOptions.CloneSet(), cloneFactory);

		if (setNullValue || resultClone != null)
		{
			_cacheKeys.Add(key, tags);
			_cache.Set(key, resultClone, (options ?? new()).RegisterPostEvictionCallback(_cacheKeys.RemoveCallback));
		}

		return result;
	}

	public T GetOrSetValuePermanently<T>(
		string key,
		Func<T> value,
		List<string>? tags = null,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null)
		=> GetOrSetValue(
			key,
			value,
			tags,
			options: null,
			forceSet,
			setNullValue,
			cloneOptions,
			cloneFactory);

	public T GetOrSetValueWithSlidingExpiration<T>(
		string key,
		Func<T> value,
		TimeSpan slidingTime,
		List<string>? tags = null,
		Microsoft.Extensions.Caching.Memory.CacheItemPriority priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null)
		=> GetOrSetValue(
			key,
			value,
			tags,
			new MemoryCacheEntryOptions
			{
				SlidingExpiration = slidingTime,
				Priority = priority
			},
			forceSet,
			setNullValue,
			cloneOptions,
			cloneFactory);

	public T GetOrSetValueWithAbsoluteExpiration<T>(
		string key,
		Func<T> value,
		DateTime keepUntil,
		List<string>? tags = null,
		Microsoft.Extensions.Caching.Memory.CacheItemPriority priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null)
		=> GetOrSetValue(
			key,
			value,
			tags,
			new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = keepUntil,
				Priority = priority
			},
			forceSet,
			setNullValue,
			cloneOptions,
			cloneFactory);

	public async Task<T> GetOrSetValueAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> value,
		List<string>? tags,
		MemoryCacheEntryOptions? options,
		bool forceSet,
		bool setNullValue,
		CacheCloneOption cloneOptions,
		ICloneFactory? cloneFactory,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);
		Throw.IfArgumentNull(value);

		var cachedValue = GetValue<T>(key, cloneOptions.CloneGet(), cloneFactory);

		if (!forceSet && cachedValue != null)
			return cachedValue;

		var result = await value(cancellationToken);
		var resultClone = Clone(result, cloneOptions.CloneSet(), cloneFactory);

		if (setNullValue || resultClone != null)
		{
			_cacheKeys.Add(key, tags);
			_cache.Set(key, resultClone, (options ?? new()).RegisterPostEvictionCallback(_cacheKeys.RemoveCallback));
		}

		return result;
	}

	public async Task<T> GetOrSetValuePermanentlyAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> value,
		List<string>? tags = null,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null,
		CancellationToken cancellationToken = default)
		=> await GetOrSetValueAsync(
			key,
			value,
			tags,
			options: null,
			forceSet,
			setNullValue,
			cloneOptions,
			cloneFactory,
			cancellationToken);

	public async Task<T> GetOrSetValueWithSlidingExpirationAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> value,
		TimeSpan slidingTime,
		List<string>? tags = null,
		Microsoft.Extensions.Caching.Memory.CacheItemPriority priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null,
		CancellationToken cancellationToken = default)
		=> await GetOrSetValueAsync(
			key,
			value,
			tags,
			new MemoryCacheEntryOptions
			{
				SlidingExpiration = slidingTime,
				Priority = priority
			},
			forceSet,
			setNullValue,
			cloneOptions,
			cloneFactory,
			cancellationToken);

	public async Task<T> GetOrSetValueWithAbsoluteExpirationAsync<T>(
		string key,
		Func<CancellationToken, Task<T>> value,
		DateTime keepUntil,
		List<string>? tags = null,
		Microsoft.Extensions.Caching.Memory.CacheItemPriority priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
		bool forceSet = false,
		bool setNullValue = false,
		CacheCloneOption cloneOptions = CacheCloneOption.CloneBeforeSetAndGet,
		ICloneFactory? cloneFactory = null,
		CancellationToken cancellationToken = default)
		=> await GetOrSetValueAsync(
			key,
			value,
			tags,
			new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = keepUntil,
				Priority = priority
			},
			forceSet,
			setNullValue,
			cloneOptions,
			cloneFactory,
			cancellationToken);

	public void RemoveValue(string key)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		_cacheKeys.Remove(key);
		_cache.Remove(key);
	}

	public void RemoveValuesForTag(string tag)
	{
		Throw.IfArgumentNullOrWhiteSpace(tag);

		var keys = _cacheKeys.GetAllKeys(tag);

		if (keys == null || keys.Count == 0)
			return;

		foreach (var key in keys)
		{
			_cacheKeys.Remove(key);
			_cache.Remove(key);
		}
	}

	public void RemoveValuesForWholeTags(List<string> tags)
	{
		Throw.IfArgumentNullOrEmpty(tags);

		var keys = _cacheKeys.GetAllKeys(tags);

		foreach (var key in keys)
		{
			_cacheKeys.Remove(key);
			_cache.Remove(key);
		}
	}

	T? Legion.Caching.IADFCache.GetValue<T>(string key, bool getClone, ICloneFactory? cloneFactory) where T : default
		=> GetValue<T>(key, getClone, cloneFactory);

	bool Legion.Caching.IADFCache.SetValuePermanently<T>(string key, T value, List<string>? tags, bool setNullValue, bool createClone, ICloneFactory? cloneFactory)
		=> SetValuePermanently(key, value, tags, setNullValue, createClone, cloneFactory);

	bool Legion.Caching.IADFCache.SetValueWithSlidingExpiration<T>(string key, T value, TimeSpan slidingTime, List<string>? tags, Legion.Caching.CacheItemPriority priority, bool setNullValue, bool createClone, ICloneFactory? cloneFactory)
		=> SetValueWithSlidingExpiration(key, value, slidingTime, tags, priority.Convert(), setNullValue, createClone, cloneFactory);

	bool Legion.Caching.IADFCache.SetValueWithAbsoluteExpiration<T>(string key, T value, DateTime keepUntil, List<string>? tags, Legion.Caching.CacheItemPriority priority, bool setNullValue, bool createClone, ICloneFactory? cloneFactory)
		=> SetValueWithAbsoluteExpiration(key, value, keepUntil, tags, priority.Convert(), setNullValue, createClone, cloneFactory);

	T IADFCache.GetOrSetValuePermanently<T>(string key, Func<T> value, List<string>? tags, bool forceSet, bool setNullValue, CacheCloneOption cloneOptions, ICloneFactory? cloneFactory)
		=> GetOrSetValuePermanently(key, value, tags, forceSet, setNullValue, cloneOptions, cloneFactory);

	T IADFCache.GetOrSetValueWithSlidingExpiration<T>(string key, Func<T> value, TimeSpan slidingTime, List<string>? tags, Caching.CacheItemPriority priority, bool forceSet, bool setNullValue, CacheCloneOption cloneOptions, ICloneFactory? cloneFactory)
		=> GetOrSetValueWithSlidingExpiration(key, value, slidingTime, tags, priority.Convert(), forceSet, setNullValue, cloneOptions, cloneFactory);

	T IADFCache.GetOrSetValueWithAbsoluteExpiration<T>(string key, Func<T> value, DateTime keepUntil, List<string>? tags, Caching.CacheItemPriority priority, bool forceSet, bool setNullValue, CacheCloneOption cloneOptions, ICloneFactory? cloneFactory)
		=> GetOrSetValueWithAbsoluteExpiration(key, value, keepUntil, tags, priority.Convert(), forceSet, setNullValue, cloneOptions, cloneFactory);

	async Task<T> IADFCache.GetOrSetValuePermanentlyAsync<T>(string key, Func<CancellationToken, Task<T>> value, List<string>? tags, bool forceSet, bool setNullValue, CacheCloneOption cloneOptions, ICloneFactory? cloneFactory, CancellationToken cancellationToken)
		=> await GetOrSetValuePermanentlyAsync(key, value, tags, forceSet, setNullValue, cloneOptions, cloneFactory, cancellationToken);

	async Task<T> IADFCache.GetOrSetValueWithSlidingExpirationAsync<T>(string key, Func<CancellationToken, Task<T>> value, TimeSpan slidingTime, List<string>? tags, Caching.CacheItemPriority priority, bool forceSet, bool setNullValue, CacheCloneOption cloneOptions, ICloneFactory? cloneFactory, CancellationToken cancellationToken)
		=> await GetOrSetValueWithSlidingExpirationAsync(key, value, slidingTime, tags, priority.Convert(), forceSet, setNullValue, cloneOptions, cloneFactory, cancellationToken);

	async Task<T> IADFCache.GetOrSetValueWithAbsoluteExpirationAsync<T>(string key, Func<CancellationToken, Task<T>> value, DateTime keepUntil, List<string>? tags, Caching.CacheItemPriority priority, bool forceSet, bool setNullValue, CacheCloneOption cloneOptions, ICloneFactory? cloneFactory, CancellationToken cancellationToken)
		=> await GetOrSetValueWithAbsoluteExpirationAsync(key, value, keepUntil, tags, priority.Convert(), forceSet, setNullValue, cloneOptions, cloneFactory, cancellationToken);

	void Legion.Caching.IADFCache.RemoveValue(string key)
		=> RemoveValue(key);

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				_cache.Dispose();
			}

			disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

public sealed class CacheKeys
{
	private readonly ConcurrentDictionary<string, List<string>> _keys; //ConcurrentDictionary<cacheKey, List<tag>>
	private readonly ConcurrentDictionary<string, List<string>> _tags; //ConcurrentDictionary<tag, List<cacheKey>>

	public CacheKeys()
	{
		_keys = [];
		_tags = [];
	}

	public List<string> GetAllKeys()
		=> _keys.Keys.ToList();

	public List<string> GetAllKeys(string tag)
	{
		Throw.IfArgumentNullOrWhiteSpace(tag);

		_tags.TryGetValue(tag, out var keys);
		return keys?.ToList() ?? [];
	}

	public List<string> GetAllKeys(List<string> tags)
	{
		Throw.IfArgumentNullOrEmpty(tags);

		List<string>? keys = null;

		foreach (var tag in tags)
		{
			if (_tags.TryGetValue(tag, out var keysList))
			{
				if (keys == null)
				{
					keys = keysList?.ToList() ?? [];
					continue;
				}

				keys = keys.Intersect(keysList).ToList();
			}
			else
			{
				return [];
			}
		}

		return keys?.ToList() ?? [];
	}

	public List<string> GetAllTags(string key)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		_keys.TryGetValue(key, out var tags);
		return tags?.ToList() ?? [];
	}

	public bool Add(string key, List<string>? tags)
	{
		Throw.IfArgumentNullOrWhiteSpace(key);

		var added = _keys.TryAdd(key, tags ?? []);

		if (added && 0 < tags?.Count)
		{
			foreach (var tag in tags)
			{
				_tags.AddOrUpdate(tag, x => [key], (x, keys) =>
				{
					lock (keys)
					{
						if (!keys.Contains(key))
							keys.Add(key);
					}

					return keys;
				});
			}
		}

		return added;
	}

	public bool Remove(string key)
		=> RemoveInternal(key, false);

	private bool RemoveInternal(string key, bool isCallback)
	{
		if (string.IsNullOrWhiteSpace(key))
			return false;

		var removed = _keys.Remove(key, out var tags);

		if (removed && 0< tags?.Count)
		{
			foreach (var tag in tags)
			{
				if (_tags.TryGetValue(tag, out var keys))
				{
					lock (keys)
					{
						keys.Remove(key);
					}
				}
			}
		}

		return removed;
	}

	public void RemoveCallback(object key, object? value, EvictionReason reason, object? state)
	{
		if (key is string stringKey)
			RemoveInternal(stringKey, true);
	}
}

public static class CacheItemPriorityExtensions
{
	public static Microsoft.Extensions.Caching.Memory.CacheItemPriority Convert(this Legion.Caching.CacheItemPriority priority)
		=> priority switch
		{
			Caching.CacheItemPriority.Low => Microsoft.Extensions.Caching.Memory.CacheItemPriority.Low,
			Caching.CacheItemPriority.Normal => Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal,
			Caching.CacheItemPriority.High => Microsoft.Extensions.Caching.Memory.CacheItemPriority.High,
			Caching.CacheItemPriority.NeverRemove => Microsoft.Extensions.Caching.Memory.CacheItemPriority.NeverRemove,
			_ => Microsoft.Extensions.Caching.Memory.CacheItemPriority.NeverRemove,
		};
}
