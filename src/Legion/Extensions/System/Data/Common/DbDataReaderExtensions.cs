namespace Legion.Extensions;

public static class DbDataReaderExtensions
{
	public static T? GetValueOrDefault<T>(this System.Data.Common.DbDataReader reader, string name)
	{
		var ordinal = reader.GetOrdinal(name);
		return reader.IsDBNull(ordinal)
			? default
			: reader.GetFieldValue<T>(ordinal);
	}

	public static T? GetValueOrDefault<T>(this System.Data.Common.DbDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal)
			? default
			: reader.GetFieldValue<T>(ordinal);
	}

	public static object? GetValueOrNull<T>(this System.Data.Common.DbDataReader reader, string name)
	{
		var ordinal = reader.GetOrdinal(name);
		return reader.IsDBNull(ordinal)
			? (object?)null
			: reader.GetFieldValue<T>(ordinal);
	}

	public static object? GetValueOrNull<T>(this System.Data.Common.DbDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal)
			? (object?)null
			: reader.GetFieldValue<T>(ordinal);
	}

	public static bool? GetNullableBoolean(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (bool?)null : reader.GetBoolean(ordinal);
	}

	public static byte? GetNullableByte(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (byte?)null : reader.GetByte(ordinal);
	}

	public static char? GetNullableChar(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (char?)null : reader.GetChar(ordinal);
	}

	public static DateTime? GetNullableDateTime(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
	}

	public static decimal? GetNullableDecimal(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (decimal?)null : reader.GetDecimal(ordinal);
	}

	public static double? GetNullableDouble(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (double?)null : reader.GetDouble(ordinal);
	}

	public static Guid? GetNullableGuid(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (Guid?)null : reader.GetGuid(ordinal);
	}

	public static short? GetNullableInt16(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (short?)null : reader.GetInt16(ordinal);
	}

	public static int? GetNullableInt32(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
	}

	public static long? GetNullableInt64(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (long?)null : reader.GetInt64(ordinal);
	}

	public static float? GetNullableFloat(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? (float?)null : reader.GetFloat(ordinal);
	}

	public static string? GetNullableString(this System.Data.IDataReader reader, int ordinal)
	{
		return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
	}
}
