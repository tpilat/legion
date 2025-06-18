using NpgsqlTypes;

namespace Npgsql;

public static class NpgsqlParameterCollectionExtensions
{
	public static void AddWithNullableValue(this NpgsqlParameterCollection collection, string parameterName, NpgsqlDbType parameterType, object? value)
		=> collection.AddWithValue(parameterName, parameterType, value ?? DBNull.Value);
}
