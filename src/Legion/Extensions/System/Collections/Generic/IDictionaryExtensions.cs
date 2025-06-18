using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Legion.Extensions;

public static class IDictionaryExtensions
{
	/// <summary>
	/// Get value from dictionary by key
	/// </summary>
	public static TValue? Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
	{
		return Get(dictionary, key, default);
	}

	/// <summary>
	/// Get value from dictionary by key
	/// </summary>
	public static TValue? Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue)
	{
		if (dictionary == null)
			return default;

		if (dictionary.TryGetValue(key, out TValue? value))
			return value;
		else
			return defaultValue;
	}

	public static TValue? AddOrGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
	{
		Throw.IfArgumentNull(dict);

		if (!dict.TryGetValue(key, out TValue? val))
		{
			val = value;
			dict.Add(key, val);
		}

		return val;
	}

	public static TValue AddOrGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory)
	{
		Throw.IfArgumentNull(dict);
		Throw.IfArgumentNull(valueFactory);

		if (!dict.TryGetValue(key, out TValue? val))
		{
			val = valueFactory(key);
			dict.Add(key, val);
		}

		return val;
	}

	public static bool AddUniqueKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
	{
		Throw.IfArgumentNull(dict);

		if (!dict.ContainsKey(key))
		{
			dict.Add(key, value);
			return true;
		}
		else
		{
			return false;
		}
	}

	public static IDictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue item)
	{
		Throw.IfArgumentNull(dict);

		if (item != null)
		{
			dict[key] = item;
		}

		return dict;
	}

	public static IDictionary<TKey, TValue> AddItem<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value, bool checkForNulls = false)
	{
		Throw.IfArgumentNull(dict);
		Throw.IfArgumentNull(key);

		if (checkForNulls && value == null)
			return dict;

		dict[key] = value;

		return dict;
	}

	public static List<TKey>? AddRangeUniqueKeys<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> dictionary)
	{
		if (dict == null || dictionary == null)
			return null;

		var result = new List<TKey>();
		foreach (KeyValuePair<TKey, TValue> item in dictionary)
		{
			if (!dict.ContainsKey(item.Key))
			{
				dict.Add(item.Key, item.Value);
				result.Add(item.Key);
			}
		}
		return result;
	}

	public static List<TKey>? AddOrReplaceRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> dictionary)
	{
		if (dict == null || dictionary == null)
			return null;

		var result = new List<TKey>();
		foreach (KeyValuePair<TKey, TValue> item in dictionary)
		{
			if (dict.ContainsKey(item.Key))
			{
				dict[item.Key] = item.Value;
			}
			else
			{
				dict.Add(item.Key, item.Value);
				result.Add(item.Key);
			}
		}
		return result;
	}

	public static bool RemoveIfKeyExists<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
	{
		if (dict == null)
			return false;

		if (dict.ContainsKey(key))
		{
			dict.Remove(key);
			return true;
		}
		else
		{
			return false;
		}
	}

	[return: NotNullIfNotNull(nameof(dict))]
	public static string? ConcatKeysAndValues<TKey, TValue>(this IDictionary<TKey, TValue> dict, string keyValueDelimiter, string keyValuePairDelimiter)
	{
		if (dict == null)
			return null;

		var sb = new StringBuilder();
		int idx = 0;
		foreach (KeyValuePair<TKey, TValue> kvp in dict)
		{
			string endDelimiter = string.Empty;
			if (idx < (dict.Count - 1)) //last
				endDelimiter = keyValuePairDelimiter;

			sb.AppendFormat("{0}{1}{2}{3}", kvp.Key, keyValueDelimiter, kvp.Value, endDelimiter);
		}

		return sb.ToString();
	}

	public static bool TryGetValue<K, V>(this IDictionary<K, object> dictionary, K key, out V? value)
	{
		Throw.IfArgumentNull(dictionary);

		if (dictionary.TryGetValue(key, out var tmp))
		{
			if (tmp is not V)
			{
				value = default;
				return false;
			}
			value = (V)tmp;
			return true;
		}
		else
		{
			value = default;
			return false;
		}
	}

	public static IDictionary<string, string> ToJson(this IDictionary<string, object> jsonObj)
	{
		return jsonObj?.ToDictionary(
			x => x.Key,
			y => y.Value as string
			?? Newtonsoft.Json.JsonConvert.SerializeObject(y.Value))
			?? [];
	}

	/// <summary>
	/// Get key and value by TValue type
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="dictionary"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool TryGetValue<TKey, TValue>(this IDictionary<TKey, object> dictionary, out TKey? key, out TValue? value)
	{
		Throw.IfArgumentNull(dictionary);

		key = default;
		value = default;

		foreach (var item in dictionary)
		{
			if (item.Value is TValue value1)
			{
				key = item.Key;
				value = value1;
				return true;
			}
		}

		return false;
	}

	public static TValue Merge<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue?, TValue> merge)
	{
		Throw.IfArgumentNull(dictionary);
		Throw.IfArgumentNull(merge);

		TValue? result;
		if (dictionary.TryGetValue(key, out TValue? oldValue))
		{
			result = merge(oldValue);
			dictionary[key] = result;
		}
		else
		{
			result = merge(default);
			dictionary[key] = result;
		}

		return result;
	}

	public static bool ContainsKey<TKey, TValue>(
		this IDictionary<TKey, TValue> dictionary,
		TKey key,
		IEqualityComparer<TKey> comparer)
		where TKey : notnull
	{
		Throw.IfArgumentNull(dictionary);
		Throw.IfArgumentNull(comparer);

		foreach (var existingKey in dictionary.Keys)
		{
			if (comparer.Equals(existingKey, key))
				return true;
		}

		return false;
	}

	public static bool ContainsKey<TValue>(
		this IDictionary<string, TValue> dictionary,
		string key,
		StringComparison comparison)
	{
		Throw.IfArgumentNull(dictionary);

		foreach (var existingKey in dictionary.Keys)
		{
			if (string.Equals(existingKey, key, comparison))
				return true;
		}

		return false;
	}

	public static bool TryGetValue<TKey, TValue>(
		this IDictionary<TKey, TValue> dictionary,
		TKey key,
		IEqualityComparer<TKey> comparer,
		out TValue value)
		where TKey : notnull
	{
		Throw.IfArgumentNull(dictionary);
		Throw.IfArgumentNull(comparer);

		foreach (var kvp in dictionary)
		{
			if (comparer.Equals(kvp.Key, key))
			{
				value = kvp.Value;
				return true;
			}
		}
		value = default!;
		return false;
	}

	public static bool TryGetValue<TValue>(
		this IDictionary<string, TValue> dictionary,
		string key,
		StringComparison comparison,
		out TValue value)
	{
		Throw.IfArgumentNull(dictionary);

		foreach (var kvp in dictionary)
		{
			if (string.Equals(kvp.Key, key, comparison))
			{
				value = kvp.Value;
				return true;
			}
		}
		value = default!;
		return false;
	}

	public static bool Remove<TKey, TValue>(
		this Dictionary<TKey, TValue> dictionary,
		TKey key,
		IEqualityComparer<TKey> comparer)
		where TKey : notnull
	{
		Throw.IfArgumentNull(dictionary);
		Throw.IfArgumentNull(comparer);

		foreach (var existingKey in dictionary.Keys)
		{
			if (comparer.Equals(existingKey, key))
			{
				return dictionary.Remove(existingKey);
			}
		}
		return false;
	}

	public static bool Remove<TValue>(
		this Dictionary<string, TValue> dictionary,
		string key,
		StringComparison comparison)
	{
		foreach (var existingKey in dictionary.Keys)
		{
			if (string.Equals(existingKey, key, comparison))
			{
				return dictionary.Remove(existingKey);
			}
		}
		return false;
	}

	public static bool ContainsKey<TKey, TValue>(
		this IReadOnlyDictionary<TKey, TValue> dictionary,
		TKey key,
		IEqualityComparer<TKey> comparer)
		where TKey : notnull
	{
		Throw.IfArgumentNull(dictionary);
		Throw.IfArgumentNull(comparer);

		foreach (var existingKey in dictionary.Keys)
		{
			if (comparer.Equals(existingKey, key))
				return true;
		}

		return false;
	}

	public static bool ContainsKey<TValue>(
		this IReadOnlyDictionary<string, TValue> dictionary,
		string key,
		StringComparison comparison)
	{
		Throw.IfArgumentNull(dictionary);

		foreach (var existingKey in dictionary.Keys)
		{
			if (string.Equals(existingKey, key, comparison))
				return true;
		}

		return false;
	}

	public static bool TryGetValue<TKey, TValue>(
		this IReadOnlyDictionary<TKey, TValue> dictionary,
		TKey key,
		IEqualityComparer<TKey> comparer,
		out TValue value)
		where TKey : notnull
	{
		Throw.IfArgumentNull(dictionary);
		Throw.IfArgumentNull(comparer);

		foreach (var kvp in dictionary)
		{
			if (comparer.Equals(kvp.Key, key))
			{
				value = kvp.Value;
				return true;
			}
		}
		value = default!;
		return false;
	}

	public static bool TryGetValue<TValue>(
		this IReadOnlyDictionary<string, TValue> dictionary,
		string key,
		StringComparison comparison,
		out TValue value)
	{
		Throw.IfArgumentNull(dictionary);

		foreach (var kvp in dictionary)
		{
			if (string.Equals(kvp.Key, key, comparison))
			{
				value = kvp.Value;
				return true;
			}
		}
		value = default!;
		return false;
	}

#if NETSTANDARD2_0 || NETSTANDARD2_1
	/// <summary>
	/// Attempts to add the specified key and value to the dictionary.
	/// </summary>
	/// <param name="key">The key of the element to add.</param>
	/// <param name="value">The value of the element to add. It can be null.</param>
	/// <param name="dictionary"></param>
	/// <returns>true if the key/value pair was added to the dictionary successfully; otherwise, false.</returns>
	/// <exception cref="System.ArgumentNullException">key is null.</exception>
	public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if (dictionary == null)
			throw new ArgumentNullException(nameof(dictionary));

		if (key == null)
			throw new ArgumentNullException(nameof(key));

		if (dictionary.ContainsKey(key))
			return false;

		dictionary.Add(key, value);

		return true;
	}



	/// <summary>
	/// Removes the value with the specified key from the System.Collections.Generic.Dictionary`2, and copies the element to the value parameter.
	/// </summary>
	/// <param name="key">The key of the element to remove.</param>
	/// <param name="value">The removed element.</param>
	/// <param name="dictionary"></param>
	/// <returns>true if the element is successfully found and removed; otherwise, false.</returns>
	/// <exception cref="System.ArgumentNullException">key is null.</exception>
	public static bool Remove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		if (dictionary == null)
			throw new ArgumentNullException(nameof(dictionary));

		if (key == null)
			throw new ArgumentNullException(nameof(key));

		if (dictionary.TryGetValue(key, out value))
		{
			dictionary.Remove(key);
			return true;
		}

		return false;
	}

	/// <summary>
	/// Returns a read-only <see cref="System.Collections.ObjectModel.ReadOnlyDictionary{TKey, TValue}"/> wrapper
	/// for the current dictionary.
	/// </summary>
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	/// <param name="dictionary">The dictionary to wrap.</param>
	/// <returns>An object that acts as a read-only wrapper around the current <see cref="IDictionary{TKey, TValue}"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is null.</exception>
	public static System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : notnull =>
		new(dictionary);
#endif
}
