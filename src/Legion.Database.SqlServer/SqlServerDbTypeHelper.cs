using Legion.Extensions;
using System.Collections.Concurrent;
using System.Data;

namespace Legion.Database.SqlServer;

public static class SqlServerDbTypeHelper
{
	private static readonly Lazy<ConcurrentDictionary<Type, SqlDbType>> _cacheTtoSql = new();
	private static readonly Lazy<ConcurrentDictionary<SqlDbType, Type>> _cacheSqlToT = new();

	public static SqlDbType GetSqlServerDbType<T>()
		where T : struct
	{
		var type = typeof(T).GetUnderlyingNullableType();

		if (_cacheTtoSql.Value.TryGetValue(type, out SqlDbType result))
			return result;

		if (type == typeof(Guid))
			result = SqlDbType.UniqueIdentifier;
		else if (type == typeof(int))
			result = SqlDbType.Int;
		else if (type == typeof(long))
			result = SqlDbType.BigInt;
		else if (type == typeof(bool))
			result = SqlDbType.Bit;
		else if (type == typeof(byte))
			result = SqlDbType.VarBinary;
		else if (type == typeof(char))
			result = SqlDbType.Char;
		else if (type == typeof(decimal))
			result = SqlDbType.Decimal;
		else if (type == typeof(double))
			result = SqlDbType.Decimal;
		else if (type == typeof(float))
			result = SqlDbType.Decimal;
		else if (type == typeof(short))
			result = SqlDbType.SmallInt;
		else if (type == typeof(DateTime))
			result = SqlDbType.Timestamp;
		else if (type == typeof(DateTimeOffset))
			result = SqlDbType.Timestamp;
#if NET6_0_OR_GREATER
		else if (type == typeof(DateOnly))
			result = SqlDbType.Timestamp;
		else if (type == typeof(TimeOnly))
			result = SqlDbType.Timestamp;
#endif
		else if (type == typeof(TimeSpan))
			result = SqlDbType.Timestamp;
		else
			throw new NotSupportedException();

		_cacheTtoSql.Value.TryAdd(type, result);
		return result;
	}

	public static Type GetCSharpType(SqlDbType sqlDbType)
	{
		if (_cacheSqlToT.Value.TryGetValue(sqlDbType, out Type? type))
			return type;

		type = sqlDbType switch
		{
			SqlDbType.BigInt => typeof(long),
			SqlDbType.Binary => typeof(byte[]),
			SqlDbType.Bit => typeof(bool),
			SqlDbType.Char => typeof(string),
			SqlDbType.DateTime => typeof(DateTime),
			SqlDbType.Decimal => typeof(decimal),
			SqlDbType.Float => typeof(double),
			SqlDbType.Image => typeof(byte[]),
			SqlDbType.Int => typeof(int),
			SqlDbType.Money => typeof(decimal),
			SqlDbType.NChar => typeof(string),
			SqlDbType.NText => typeof(string),
			SqlDbType.NVarChar => typeof(string),
			SqlDbType.Real => typeof(float),
			SqlDbType.UniqueIdentifier => typeof(Guid),
			SqlDbType.SmallDateTime => typeof(DateTime),
			SqlDbType.SmallInt => typeof(short),
			SqlDbType.SmallMoney => typeof(decimal),
			SqlDbType.Text => typeof(string),
			SqlDbType.Timestamp => typeof(byte[]),
			SqlDbType.TinyInt => typeof(Byte),
			SqlDbType.VarBinary => typeof(byte[]),
			SqlDbType.VarChar => typeof(string),
			SqlDbType.Variant => typeof(object),
			SqlDbType.Xml => typeof(string),
			SqlDbType.Udt => typeof(object),
			SqlDbType.Structured => typeof(object),
#if NET6_0_OR_GREATER
			SqlDbType.Date => typeof(DateOnly),
			SqlDbType.Time => typeof(TimeOnly),
#endif
			SqlDbType.DateTime2 => typeof(DateTime),
			SqlDbType.DateTimeOffset => typeof(DateTimeOffset),
			_ => throw new NotSupportedException(),
		};

		_cacheSqlToT.Value.TryAdd(sqlDbType, type);
		return type;
	}
}
