using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;

namespace Legion;

public class GlobalCache
{
	private static ConcurrentDictionary<string, object?>? _dataStore;

	public static bool TryGetData(string name, out object? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		value = null;
		if (_dataStore == null)
			return false;

		return _dataStore.TryGetValue(name, out value);
	}

	public static bool TryGetData<T>(string name, out T? value)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		value = default;
		if (_dataStore == null)
			return false;

		var exists = _dataStore.TryGetValue(name, out var val);
		if (exists)
			value = (T?)val;

		return exists;
	}

	public static void SetData(string name, object? data)
	{
		Throw.IfArgumentNullOrWhiteSpace(name);

		if (_dataStore == null)
			Interlocked.CompareExchange(ref _dataStore, [], null);

		_dataStore.AddOrUpdate(name, data, (k, v) => data);
	}

	private static readonly Lazy<UTF8Encoding> _utf8NoBOM = new(() => new UTF8Encoding(false));
	public static UTF8Encoding UTF8NoBOM => _utf8NoBOM.Value;

	private static readonly Lazy<JsonSerializerSettings> _jsonSerializerSettings_WithRecursiveObjs = new(() =>
		new JsonSerializerSettings
		{
			Formatting = Formatting.None,
			ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
			PreserveReferencesHandling = PreserveReferencesHandling.Objects, //PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling.All,
			MaxDepth = 255,
		});

	public static JsonSerializerSettings JsonSerializerSettings_WithRecursiveObjs => _jsonSerializerSettings_WithRecursiveObjs.Value;

	private static readonly Lazy<JsonSerializerSettings> _jsonSerializerSettings_WithRecursiveObjsAndPrivateCtor = new(() =>
		new JsonSerializerSettings
		{
			Formatting = Formatting.None,
			ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
			PreserveReferencesHandling = PreserveReferencesHandling.Objects, //PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling.All,
			MaxDepth = 255,
			ContractResolver = new Legion.Serializer.JsonConverters.PrivateSetterContractResolver()
		});
	public static JsonSerializerSettings JsonSerializerSettings_WithRecursiveObjsAndPrivateCtor => _jsonSerializerSettings_WithRecursiveObjsAndPrivateCtor.Value;
}
