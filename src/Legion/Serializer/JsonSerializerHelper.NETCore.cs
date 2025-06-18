namespace Legion.Serializer;

#if NET5_0_OR_GREATER
public static partial class JsonSerializerHelper
{
	public static object? Deserialize(
		Stream stream,
		Type type,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNull(stream);
		Throw.IfArgumentNull(type);

		var result = System.Text.Json.JsonSerializer.Deserialize(stream, type, jsonSerializerOptions);
		return result;
	}

	public static T? Deserialize<T>(
		Stream stream,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNull(stream);

		var result = System.Text.Json.JsonSerializer.Deserialize<T>(stream, jsonSerializerOptions);
		return result;
	}

	public static object? Deserialize(
		string json,
		Type type,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNullOrWhiteSpace(json);
		Throw.IfArgumentNull(type);

		var result = System.Text.Json.JsonSerializer.Deserialize(json, type, jsonSerializerOptions);
		return result;
	}

	public static T? Deserialize<T>(
		string json,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNullOrWhiteSpace(json);

		var result = System.Text.Json.JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
		return result;
	}

	public static void Serialize(
		object? obj,
		Type type,
		Stream utf8JsonOutputStream,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		System.Text.Json.JsonSerializer.Serialize(utf8JsonOutputStream, obj, type, jsonSerializerOptions);
	}

	public static void Serialize<T>(
		T obj,
		Stream utf8JsonOutputStream,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		System.Text.Json.JsonSerializer.Serialize(utf8JsonOutputStream, obj, jsonSerializerOptions);
	}

	//public static void Serialize(
	//	object? obj,
	//	Type type,
	//	StreamWriter streamWriter,
	//	System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	//{
	//	Throw.ArgumentNull(obj);
	//	Throw.ArgumentNull(type);
	//	Throw.ArgumentNull(streamWriter);

	//	System.Text.Json.JsonSerializer.Serialize(streamWriter, obj, type, jsonSerializerOptions);
	//}

	//public static void Serialize<T>(
	//	T obj,
	//	StreamWriter streamWriter,
	//	System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	//{
	//	Throw.ArgumentNull(obj);
	//	Throw.ArgumentNull(streamWriter);

	//	System.Text.Json.JsonSerializer.Serialize(streamWriter, obj, jsonSerializerOptions);
	//}

	public static string Serialize(
		object? obj,
		Type type,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);

		var result = System.Text.Json.JsonSerializer.Serialize(obj, type, jsonSerializerOptions);
		return result;
	}

	public static string Serialize<T>(
		T obj,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions)
	{
		Throw.IfArgumentNull(obj);

		var result = System.Text.Json.JsonSerializer.Serialize(obj, jsonSerializerOptions);
		return result;
	}

	public static async Task<object?> DeserializeAsync(
		Stream stream,
		Type type,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(stream);
		Throw.IfArgumentNull(type);

		var result = await System.Text.Json.JsonSerializer.DeserializeAsync(stream, type, jsonSerializerOptions, cancellationToken);
		return result;
	}

	public static async Task<T?> DeserializeAsync<T>(
		Stream stream,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(stream);

		var result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions, cancellationToken);
		return result;
	}

	public static async Task SerializeAsync(
		object? obj,
		Type type,
		Stream utf8JsonOutputStream,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(type);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		await System.Text.Json.JsonSerializer.SerializeAsync(utf8JsonOutputStream, obj, type, jsonSerializerOptions, cancellationToken);
	}

	public static async Task SerializeAsync<T>(
		T obj,
		Stream utf8JsonOutputStream,
		System.Text.Json.JsonSerializerOptions? jsonSerializerOptions,
		CancellationToken cancellationToken = default)
	{
		Throw.IfArgumentNull(obj);
		Throw.IfArgumentNull(utf8JsonOutputStream);

		await System.Text.Json.JsonSerializer.SerializeAsync(utf8JsonOutputStream, obj, jsonSerializerOptions, cancellationToken);
	}

	//public static async Task SerializeAsync(
	//	object? obj,
	//	Type type,
	//	StreamWriter streamWriter,
	//	System.Text.Json.JsonSerializerOptions? jsonSerializerOptions,
	//	CancellationToken cancellationToken = default)
	//{
	//	Throw.ArgumentNull(obj);
	//	Throw.ArgumentNull(type);
	//	Throw.ArgumentNull(streamWriter);

	//	await System.Text.Json.JsonSerializer.SerializeAsync(streamWriter, obj, type, jsonSerializerOptions, cancellationToken);
	//}

	//public static async Task SerializeAsync<T>(
	//	T obj,
	//	StreamWriter streamWriter,
	//	System.Text.Json.JsonSerializerOptions? jsonSerializerOptions,
	//	CancellationToken cancellationToken = default)
	//{
	//	Throw.ArgumentNull(obj);
	//	Throw.ArgumentNull(streamWriter);

	//	await System.Text.Json.JsonSerializer.SerializeAsync(streamWriter, obj, jsonSerializerOptions, cancellationToken);
	//}
}
#endif
