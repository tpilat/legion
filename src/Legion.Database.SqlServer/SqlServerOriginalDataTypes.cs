using Legion.Enums;
using Legion.Extensions;

namespace Legion.Database.SqlServer;

public static class SqlServerOriginalDataTypes
{
	public const string _bigint = "bigint";
	public const string _binary = "binary";
	public const string _bit = "bit";
	public const string _char = "char";
	public const string _date = "date";
	public const string _datetime = "datetime";
	public const string _datetime2 = "datetime2";
	public const string _datetimeoffset = "datetimeoffset";
	public const string _decimal = "decimal";
	public const string _float = "float";
	public const string _image = "image";
	public const string _int = "int";
	public const string _money = "money";
	public const string _nchar = "nchar";
	public const string _ntext = "ntext";
	public const string _numeric = "numeric";
	public const string _nvarchar = "nvarchar";
	public const string _real = "real";
	public const string _smalldatetime = "smalldatetime";
	public const string _smallint = "smallint";
	public const string _smallmoney = "smallmoney";
	public const string _text = "text";
	public const string _time = "time";
	public const string _timestamp = "timestamp";
	public const string _tinyint = "tinyint";
	public const string _uniqueidentifier = "uniqueidentifier";
	public const string _varbinary = "varbinary";
	public const string _varchar = "varchar";
	public const string _xml = "xml";

	public const string _geography = "geography";
	public const string _geometry = "geometry";
	public const string _hierarchyid = "hierarchyid";
	public const string _sql_variant = "sql_variant";
	public const string _sysname = "sysname";

	private static readonly Dictionary<SqlServerDataTypes, string> _dict;

	static SqlServerOriginalDataTypes()
	{
		_dict = new()
		{
			{ SqlServerDataTypes._bigint, _bigint },
			{ SqlServerDataTypes._binary, _binary },
			{ SqlServerDataTypes._bit, _bit },
			{ SqlServerDataTypes._char, _char },
			{ SqlServerDataTypes._date, _date },
			{ SqlServerDataTypes._datetime, _datetime },
			{ SqlServerDataTypes._datetime2, _datetime2 },
			{ SqlServerDataTypes._datetimeoffset, _datetimeoffset },
			{ SqlServerDataTypes._decimal, _decimal },
			{ SqlServerDataTypes._float, _float },
			{ SqlServerDataTypes._image, _image },
			{ SqlServerDataTypes._int, _int },
			{ SqlServerDataTypes._money, _money },
			{ SqlServerDataTypes._nchar, _nchar },
			{ SqlServerDataTypes._ntext, _ntext },
			{ SqlServerDataTypes._numeric, _numeric },
			{ SqlServerDataTypes._nvarchar, _nvarchar },
			{ SqlServerDataTypes._real, _real },
			{ SqlServerDataTypes._smalldatetime, _smalldatetime },
			{ SqlServerDataTypes._smallint, _smallint },
			{ SqlServerDataTypes._smallmoney, _smallmoney },
			{ SqlServerDataTypes._text, _text },
			{ SqlServerDataTypes._time, _time },
			{ SqlServerDataTypes._timestamp, _timestamp },
			{ SqlServerDataTypes._tinyint, _tinyint },
			{ SqlServerDataTypes._uniqueidentifier, _uniqueidentifier },
			{ SqlServerDataTypes._varbinary, _varbinary },
			{ SqlServerDataTypes._varchar, _varchar },
			{ SqlServerDataTypes._xml, _xml },
			{ SqlServerDataTypes._geography, _geography },
			{ SqlServerDataTypes._geometry, _geometry },
			{ SqlServerDataTypes._hierarchyid, _hierarchyid },
			{ SqlServerDataTypes._sql_variant, _sql_variant },
			{ SqlServerDataTypes._sysname, _sysname }
		};
	}

	public static SqlServerDataTypes? ToSqlServerDataType(string storeType)
	{
		if (string.IsNullOrWhiteSpace(storeType))
			return null;

		storeType = $"_{storeType.Replace(" ", "_").Replace("\"", "").Replace("[]", "_array")}";

		if (storeType.StartsWith("__"))
			storeType = storeType.TrimPrefix("_");

		return EnumHelper.ConvertStringToEnum<SqlServerDataTypes>(storeType, true);
	}

	public static string ToOriginalDataType(SqlServerDataTypes sqlServerDataTypes)
		=> _dict[sqlServerDataTypes];

	public static string ToOriginalDataType(string storeType)
	{
		var data_type = ToSqlServerDataType(storeType);
		if (!data_type.HasValue)
			return null!;

		return _dict[data_type.Value];
	}

	public static Type? StoreTypeToCsharpType(string storeType)
	{
		var data_type = ToSqlServerDataType(storeType);
		if (!data_type.HasValue)
			return null;

		return StoreTypeToCsharpType(data_type.Value);
	}

	public static Type? StoreTypeToCsharpType(SqlServerDataTypes storeType)
		=> storeType switch
		{
			SqlServerDataTypes._bigint => typeof(long),
			SqlServerDataTypes._binary => typeof(byte[]),
			SqlServerDataTypes._bit => typeof(bool),
			SqlServerDataTypes._char => typeof(string),
			SqlServerDataTypes._date => typeof(DateTime),
			SqlServerDataTypes._datetime => typeof(DateTime),
			SqlServerDataTypes._datetime2 => typeof(DateTime),
			SqlServerDataTypes._datetimeoffset => typeof(DateTimeOffset),
			SqlServerDataTypes._decimal => typeof(decimal),
			SqlServerDataTypes._float => typeof(double),
			SqlServerDataTypes._image => typeof(byte[]),
			SqlServerDataTypes._int => typeof(int),
			SqlServerDataTypes._money => typeof(decimal),
			SqlServerDataTypes._nchar => typeof(string),
			SqlServerDataTypes._ntext => typeof(string),
			SqlServerDataTypes._numeric => typeof(decimal),
			SqlServerDataTypes._nvarchar => typeof(string),
			SqlServerDataTypes._real => typeof(float),
			SqlServerDataTypes._smalldatetime => typeof(DateTime),
			SqlServerDataTypes._smallint => typeof(short),
			SqlServerDataTypes._smallmoney => typeof(decimal),
			SqlServerDataTypes._text => typeof(string),
			SqlServerDataTypes._time => typeof(TimeSpan),
			SqlServerDataTypes._timestamp => typeof(byte[]),
			SqlServerDataTypes._tinyint => typeof(byte),
			SqlServerDataTypes._uniqueidentifier => typeof(Guid),
			SqlServerDataTypes._varbinary => typeof(byte[]),
			SqlServerDataTypes._varchar => typeof(string),
			SqlServerDataTypes._xml => typeof(string),

			SqlServerDataTypes._sql_variant => typeof(object),
			SqlServerDataTypes._sysname => typeof(string),

			//SqlServerDataTypes._geography => typeof(Microsoft.SqlServer.Types.SqlGeography),
			//SqlServerDataTypes._geometry => typeof(Microsoft.SqlServer.Types.SqlGeometry),
			//SqlServerDataTypes._hierarchyid => typeof(Microsoft.SqlServer.Types.SqlHierarchyId),

			_ => null,
		};
}
