using Microsoft.Data.SqlClient;

namespace Legion.Extensions;

public static class SqlServerDataReaderExtensions
{
	public static Stream? GetNullableStream(this SqlDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? null : reader.GetStream(ordinal);
	}

	public static async Task<Stream?> GetNullableStreamAsync(this SqlDataReader reader, int ordinal)
	{
		return (await reader.IsDBNullAsync(ordinal).ConfigureAwait(false))
			? null
			: reader.GetStream(ordinal);
	}

	public static TimeSpan? GetNullableTimeSpan(this SqlDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (TimeSpan?)null : reader.GetTimeSpan(ordinal);
	}
}
