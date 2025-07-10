using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legion.ADF.Cache.Services;

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

		if (removed && 0 < tags?.Count)
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
