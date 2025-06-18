namespace Legion.Serializer;

public static partial class JsonSerializerHelper
{
	public static object? Deserialize(
		Stream stream,
		Type type,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return Deserialize(
				stream,
				type,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return Deserialize(
				stream,
				type,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return null;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static T? Deserialize<T>(
		Stream stream,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return Deserialize<T>(
				stream,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return Deserialize<T>(
				stream,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return default;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static object? Deserialize(
		string json,
		Type type,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return Deserialize(
				json,
				type,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return Deserialize(
				json,
				type,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return null;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static T? Deserialize<T>(
		string json,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return Deserialize<T>(
				json,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return Deserialize<T>(
				json,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return default;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static void Serialize(
		object? obj,
		Type type,
		Stream utf8JsonOutputStream,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			Serialize(
				obj,
				type,
				utf8JsonOutputStream,
				jsonSerializerSettings: null);

			return;
		}

#if NET5_0_OR_GREATER
		else
		{
			Serialize(
				obj,
				type,
				utf8JsonOutputStream,
				jsonSerializerOptions: null);

			return;
		}
#endif
	}

	public static void Serialize<T>(
		T obj,
		Stream utf8JsonOutputStream,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			Serialize(
				obj,
				utf8JsonOutputStream,
				jsonSerializerSettings: null);

			return;
		}

#if NET5_0_OR_GREATER
		else
		{
			Serialize(
				obj,
				utf8JsonOutputStream,
				jsonSerializerOptions: null);

			return;
		}
#endif
	}

	public static string Serialize(
		object? obj,
		Type type,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return Serialize(
				obj,
				type,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return Serialize(
				obj,
				type,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return default!;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static string Serialize<T>(
		T obj,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return Serialize(
				obj,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return Serialize(
				obj,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return default!;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static Task<object?> DeserializeAsync(
		Stream stream,
		Type type,
		bool useNewtonsoft = true,
		CancellationToken cancellationToken = default)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return DeserializeAsync(
				stream,
				type,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return DeserializeAsync(
				stream,
				type,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return null;
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static Task<T?> DeserializeAsync<T>(
		Stream stream,
		bool useNewtonsoft = true,
		CancellationToken cancellationToken = default)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return DeserializeAsync<T>(
				stream,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return DeserializeAsync<T>(
				stream,
				jsonSerializerOptions: null);
		}
#endif

#pragma warning disable CS0162 // Unreachable code detected
		return Task.FromResult((T?)default);
#pragma warning restore CS0162 // Unreachable code detected
	}

	public static Task SerializeAsync(
		object? obj,
		Type type,
		Stream utf8JsonOutputStream,
		bool useNewtonsoft = true,
		CancellationToken cancellationToken = default)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return SerializeAsync(
				obj,
				type,
				utf8JsonOutputStream,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return SerializeAsync(
				obj,
				type,
				utf8JsonOutputStream,
				jsonSerializerOptions: null);
		}
#endif
		return Task.CompletedTask;
	}

	public static Task SerializeAsync<T>(
		T obj,
		Stream utf8JsonOutputStream,
		bool useNewtonsoft = true)
	{
#if NETSTANDARD2_0 || NETSTANDARD2_1
		useNewtonsoft = true;
#endif

		if (useNewtonsoft)
		{
			return SerializeAsync(
				obj,
				utf8JsonOutputStream,
				jsonSerializerSettings: null);
		}

#if NET5_0_OR_GREATER
		else
		{
			return SerializeAsync(
				obj,
				utf8JsonOutputStream,
				jsonSerializerOptions: null);
		}
#endif

		return Task.CompletedTask;
	}
}
