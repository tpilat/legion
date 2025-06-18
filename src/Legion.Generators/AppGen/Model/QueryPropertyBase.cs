using Legion.Extensions;
using Legion.Generators.AppGen.Helpers;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class QueryPropertyBaseExtension
{
	[JsonProperty]
	public bool NameExtended { get; set; }

	[JsonProperty]
	public bool IsNullableExtended { get; set; }

	[JsonProperty]
	public bool MaxLengthExtended { get; set; }

	[JsonProperty]
	public bool PrecisionExtended { get; set; }

	[JsonProperty]
	public bool ScaleExtended { get; set; }

	[JsonProperty]
	public bool HasJsonConversionExtended { get; set; }
}

[Serializable]
public class QueryPropertyBase : ICommonBaseModel
{
	[JsonProperty]
	public QueryPropertyBaseExtension Extension { get; set; }

	[JsonProperty]
	private DBAbstractions.Metamodel.Property _column;

	[JsonProperty]
	public string ID { get; set; }

	[JsonIgnore]
	public Type ClrType { get; set; }

	[JsonIgnore]
	public Type UnderlyingNullableType { get; set; }

	[JsonIgnore]
	public string CSharpType { get; set; }

	[JsonProperty]
	public string Name { get; set; }

	[JsonIgnore]
	public string ColumnName => _column.ColumnName;

	[JsonIgnore]
	public string ColumnType => _column.ColumnType;

	[JsonIgnore]
	public object DefaultValue => _column.DefaultValue;

	[JsonIgnore]
	public string DefaultValueSql => _column.DefaultValueSql;

	[JsonIgnore]
	public string ComputedColumnSql => _column.ComputedColumnSql;

	[JsonIgnore]
	public string ConfiguredColumnType => _column.ConfiguredColumnType;

	[JsonProperty]
	public bool IsNullable { get; set; }

	[JsonIgnore]
	public bool? IsUnicode => _column.IsUnicode;

	[JsonProperty]
	public int? MaxLength { get; set; }

	[JsonProperty]
	public int? Precision { get; set; }

	[JsonProperty]
	public int? Scale { get; set; }

	[JsonProperty]
	public int PropertyOrdinal { get; set; }

	[JsonProperty]
	public QueryEntityBase DeclaringEntity { get; set; }

	[JsonIgnore]
	public List<string> Namespaces { get; set; }

	[JsonProperty]
	public bool HasJsonConversion { get; set; }




	[JsonIgnore]
	public bool IsRequiredEfMapping => !IsNullable && ClrType.IsNullable();

	[JsonIgnore]
	public bool IsExplicitName => ColumnName != null && ColumnName != Name;


	public QueryPropertyBase()
	{
		Namespaces = new List<string>();
	}
	
	internal void Init(DBAbstractions.Metamodel.Property column, QueryPropertyBase? ext)
	{
		_column = column ?? throw new ArgumentNullException(nameof(column));
		ID = _column.ColumnName;
		Namespaces = new List<string>(_column.Namespaces);

		Name = _column.Name;
		PropertyOrdinal = _column.ColumnOrdinal;
		IsNullable = _column.IsNullable;
		MaxLength = _column.MaxLength;
		Precision = _column.Precision;
		Scale = _column.Scale;
		Extension = new QueryPropertyBaseExtension();

		if (PropertyOrdinal == 1 && Name.StartsWith("Id", StringComparison.OrdinalIgnoreCase))
			IsNullable = false;
		
		if (ext != null)
		{
			if (ext.Extension != null)
				Extension = ext.Extension;

			if (Extension.NameExtended && !string.IsNullOrWhiteSpace(ext.Name))
				Name = ext.Name;

			if (Extension.IsNullableExtended) IsNullable = ext.IsNullable;
			if (Extension.HasJsonConversionExtended) HasJsonConversion = ext.HasJsonConversion;
		}

		var type = _column.ClrType;

		if (!IsNullable)
			type = type.GetUnderlyingNullableType();

		if (type == typeof(char) || type == typeof(char?))
			type = typeof(string);

		ClrType = HasJsonConversion
			? typeof(List<>).MakeGenericType(type)
			: type;

		var underlyingNullableType = ClrType.GetUnderlyingNullableType();
		UnderlyingNullableType = underlyingNullableType;

		CSharpType = TypeHelper.TypeToCSharpSourceCode(ClrType);
	}

	public string GetDatabaseDataType(bool includeNullability)
		=> SqlHelper.DBTypeToNullableType(ColumnType, includeNullability, IsNullable, MaxLength);

	public void ResetName()
	{
		Name = _column.Name;
	}

	public override string ToString()
	{
		return ColumnName;
	}
}
