using Legion.Enums;
using Legion.Extensions;

namespace Legion.Database.PostgreSQL;

public static class PostgreSQLOriginalDataTypes
{
	public const string _bigint = "bigint";
	public const string _boolean = "boolean";
	public const string _bytea = "bytea";
	public const string _date = "date";
	public const string _double_precision = "double precision";
	public const string _character = "character";
	public const string _character_varying = "character varying";
	public const string _integer = "integer";
	public const string _interval = "interval";
	public const string _jsonb = "jsonb";
	public const string _numeric = "numeric";
	public const string _real = "real";
	public const string _smallint = "smallint";
	public const string _text = "text";
	public const string _timestamp_with_time_zone = "timestamp with time zone";
	public const string _timestamp_without_time_zone = "timestamp without time zone";
	public const string _tsvector = "tsvector";
	public const string _uuid = "uuid";
	public const string _xml = "xml";
	public const string _text_array = "_text_array";

	private static readonly Dictionary<PostgreSQLDataTypes, string> _dict;

	static PostgreSQLOriginalDataTypes()
	{
		_dict = new()
		{
			{ PostgreSQLDataTypes._bigint, _bigint },
			{ PostgreSQLDataTypes._boolean, _boolean },
			{ PostgreSQLDataTypes._bytea, _bytea },
			{ PostgreSQLDataTypes._date, _date },
			{ PostgreSQLDataTypes._double_precision, _double_precision },
			{ PostgreSQLDataTypes._character, _character },
			{ PostgreSQLDataTypes._character_varying, _character_varying },
			{ PostgreSQLDataTypes._integer, _integer },
			{ PostgreSQLDataTypes._interval, _interval },
			{ PostgreSQLDataTypes._jsonb, _jsonb },
			{ PostgreSQLDataTypes._numeric, _numeric },
			{ PostgreSQLDataTypes._real, _real },
			{ PostgreSQLDataTypes._smallint , _smallint },
			{ PostgreSQLDataTypes._text, _text },
			{ PostgreSQLDataTypes._timestamp_with_time_zone, _timestamp_with_time_zone },
			{ PostgreSQLDataTypes._timestamp_without_time_zone, _timestamp_without_time_zone },
			{ PostgreSQLDataTypes._tsvector, _tsvector },
			{ PostgreSQLDataTypes._uuid, _uuid },
			{ PostgreSQLDataTypes._xml, _xml },
			{ PostgreSQLDataTypes._text_array, _text_array },
		};
	}

	public static PostgreSQLDataTypes? ToPostgreSQLDataType(string storeType)
	{
		if (string.IsNullOrWhiteSpace(storeType))
			return null;

		storeType = $"_{storeType.Replace(" ", "_").Replace("\"", "").Replace("[]", "_array")}";

		if (storeType.StartsWith("__"))
			storeType = storeType.TrimPrefix("_");

		return EnumHelper.ConvertStringToEnum<PostgreSQLDataTypes>(storeType, true);
	}

	public static string ToOriginalDataType(PostgreSQLDataTypes postgreSQLDataTypes)
		=> _dict[postgreSQLDataTypes];

	public static string ToOriginalDataType(string storeType)
	{
		var data_type = ToPostgreSQLDataType(storeType);
		if (!data_type.HasValue)
			return null!; 
		
		return _dict[data_type.Value];
	}

	public static Type? StoreTypeToCsharpType(string storeType)
	{
		var data_type = ToPostgreSQLDataType(storeType);
		if (!data_type.HasValue)
			return null;

		return StoreTypeToCsharpType(data_type.Value);
	}

	public static Type? StoreTypeToCsharpType(PostgreSQLDataTypes storeType)
		=> storeType switch
		{
			PostgreSQLDataTypes._bigint => typeof(long),
			PostgreSQLDataTypes._boolean => typeof(bool),
			PostgreSQLDataTypes._bytea => typeof(byte[]),
			PostgreSQLDataTypes._date => typeof(DateTime),
			PostgreSQLDataTypes._double_precision => typeof(double),
			PostgreSQLDataTypes._character => typeof(string),
			PostgreSQLDataTypes._character_varying => typeof(string),
			PostgreSQLDataTypes._integer => typeof(int),
			PostgreSQLDataTypes._interval => typeof(TimeSpan),
			PostgreSQLDataTypes._jsonb => typeof(string),
			PostgreSQLDataTypes._numeric => typeof(decimal),
			PostgreSQLDataTypes._real => typeof(float),
			PostgreSQLDataTypes._smallint => typeof(short),
			PostgreSQLDataTypes._text => typeof(string),
			PostgreSQLDataTypes._timestamp_with_time_zone
				or PostgreSQLDataTypes._timestamp_without_time_zone
					=> typeof(DateTime),
			PostgreSQLDataTypes._tsvector => typeof(string),
			PostgreSQLDataTypes._uuid => typeof(Guid),
			PostgreSQLDataTypes._xml => typeof(string),
			PostgreSQLDataTypes._text_array => typeof(List<string>),
			_ => null,
		};
}
