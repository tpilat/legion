using Npgsql;

namespace Legion.Extensions;

public static class NpgsqlDataReaderExtensions
{
	public static Stream? GetNullableStream(this NpgsqlDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? null : reader.GetStream(ordinal);
	}

	public static async Task<Stream?> GetNullableStreamAsync(this NpgsqlDataReader reader, int ordinal)
	{
		return (await reader.IsDBNullAsync(ordinal).ConfigureAwait(false))
			? null
			: (await reader.GetStreamAsync(ordinal).ConfigureAwait(false));
	}

	public static TimeSpan? GetNullableTimeSpan(this NpgsqlDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (TimeSpan?)null : reader.GetTimeSpan(ordinal);
	}
}
