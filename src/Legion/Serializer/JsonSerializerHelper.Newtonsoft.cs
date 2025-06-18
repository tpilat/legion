namespace Legion.Serializer;

public static partial class JsonSerializerHelper
{
	public static object? Deserialize(
		Stream stream,
		Type type,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(stream);
		Throw.IfArgumentNull(type);

		using var streamReader = new StreamReader(stream);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);

		var result = serializer.Deserialize(streamReader, type);
		return result;
	}

	public static T? Deserialize<T>(
		Stream stream,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(stream);

		using var streamReader = new StreamReader(stream);
		using var jsonReader = new Newtonsoft.Json.JsonTextReader(streamReader);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);

		var result = serializer.Deserialize<T>(jsonReader);
		return result;
	}

	public static object? Deserialize(
		string json,
		Type type,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNullOrWhiteSpace(json);
		Throw.IfArgumentNull(type);

		var result = Newtonsoft.Json.JsonConvert.DeserializeObject(json, type, jsonSerializerSettings);
		return result;
	}

	public static T? Deserialize<T>(
		string json,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNullOrWhiteSpace(json);

		var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
		return result;
	}

	public static void Serialize(
		object? obj,
		Type type,
		Stream utf8JsonOutputStream,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		using var streamWriter = new StreamWriter(utf8JsonOutputStream);
		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj, type);
		jsonTextWriter.Flush();
	}

	public static void Serialize<T>(
		T obj,
		Stream utf8JsonOutputStream,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		using var streamWriter = new StreamWriter(utf8JsonOutputStream);
		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj);
		jsonTextWriter.Flush();
	}

	public static void Serialize(
		object? obj,
		Type type,
		StreamWriter streamWriter,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(streamWriter);

		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj, type);
		jsonTextWriter.Flush();
	}

	public static void Serialize<T>(
		T obj,
		StreamWriter streamWriter,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(streamWriter);

		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj);
		jsonTextWriter.Flush();
	}

	public static string Serialize(
		object? obj,
		Type type,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);

		var result = Newtonsoft.Json.JsonConvert.SerializeObject(obj, type, jsonSerializerSettings);
		return result;
	}

	public static string Serialize<T>(
		T obj,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings)
	{
		Throw.IfArgumentNull(obj);

		var result = Newtonsoft.Json.JsonConvert.SerializeObject(obj, jsonSerializerSettings);
		return result;
	}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
	public static async Task<object?> DeserializeAsync(
		Stream stream,
		Type type,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(stream);
		Throw.IfArgumentNull(type);

		using var streamReader = new StreamReader(stream);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);

		var result = serializer.Deserialize(streamReader, type);
		return result;
	}

	public static async Task<T?> DeserializeAsync<T>(
		Stream stream,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(stream);

		using var streamReader = new StreamReader(stream);
		using var jsonReader = new Newtonsoft.Json.JsonTextReader(streamReader);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);

		var result = serializer.Deserialize<T>(jsonReader);
		return result;
	}

	public static async Task SerializeAsync(
		object? obj,
		Type type,
		Stream utf8JsonOutputStream,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		using var streamWriter = new StreamWriter(utf8JsonOutputStream);
		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj, type);
		jsonTextWriter.Flush();
	}

	public static async Task SerializeAsync<T>(
		T obj,
		Stream utf8JsonOutputStream,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		using var streamWriter = new StreamWriter(utf8JsonOutputStream);
		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj);
		jsonTextWriter.Flush();
	}

	public static async Task SerializeAsync(
		object? obj,
		Type type,
		StreamWriter streamWriter,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(streamWriter);

		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj, type);
		jsonTextWriter.Flush();
	}

	public static async Task SerializeAsync<T>(
		T obj,
		StreamWriter streamWriter,
		Newtonsoft.Json.JsonSerializerSettings? jsonSerializerSettings,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(streamWriter);

		using var jsonTextWriter = new Newtonsoft.Json.JsonTextWriter(streamWriter);

		var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
		serializer.Serialize(jsonTextWriter, obj);
		jsonTextWriter.Flush();
	}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
