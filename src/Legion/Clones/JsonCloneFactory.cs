using Legion.Serializer;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Legion.Clones;

public class JsonCloneFactory : ICloneFactory
{
	private static readonly Lazy<JsonSerializerSettings> _jsonSerializerSerializerSettings;
	private static readonly Lazy<JsonSerializerSettings> _jsonDeserializerSerializerSettings;

	public static JsonSerializerSettings JsonSerializerSerializerSettings => _jsonSerializerSerializerSettings.Value;
	public static JsonSerializerSettings JsonDeserializerSerializerSettings => _jsonDeserializerSerializerSettings.Value;

	static JsonCloneFactory()
	{
		_jsonSerializerSerializerSettings = new(() =>
			new JsonSerializerSettings
			{
				Formatting = Formatting.None,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
				PreserveReferencesHandling = PreserveReferencesHandling.Objects, //PreserveReferencesHandling.All,
				TypeNameHandling = TypeNameHandling.All,
				MaxDepth = 255,
			});

		_jsonDeserializerSerializerSettings = new(() =>
			new JsonSerializerSettings
			{
				Formatting = Formatting.None,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
				PreserveReferencesHandling = PreserveReferencesHandling.Objects, //PreserveReferencesHandling.All,
				TypeNameHandling = TypeNameHandling.All,
				MaxDepth = 255,
				ContractResolver = new Legion.Serializer.JsonConverters.PrivateSetterContractResolver()
			});
	}

	[return: NotNullIfNotNull(nameof(@object))]
	public T? Clone<T>(T? @object)
	{
		if (@object == null)
			return @object;

		var json = JsonSerializerHelper.Serialize(@object, _jsonSerializerSerializerSettings.Value);
		var newObject = JsonSerializerHelper.Deserialize<T>(json, _jsonDeserializerSerializerSettings.Value);
		return newObject!;
	}
}
