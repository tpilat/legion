using Legion.Database.Internal;
using Legion.Extensions;
using Legion.Generators.AppGen.Helpers;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class PropertyBaseExtension
{
	[JsonProperty]
	public bool NameExtended { get; set; }

	[JsonProperty]
	public bool MaxLengthExtended { get; set; }

	[JsonProperty]
	public bool PrecisionExtended { get; set; }

	[JsonProperty]
	public bool ScaleExtended { get; set; }

	[JsonProperty]
	public bool IsSingleUniqueConstraintExtended { get; set; }

	[JsonProperty]
	public bool IsConcurrencyTokenExtended { get; set; }

	[JsonProperty]
	public bool IsIgnoredExtended { get; set; }

	[JsonProperty]
	public bool HasJsonConversionExtended { get; set; }
}

[Serializable]
public class PropertyBase : ICommonBaseModel
{
	[JsonProperty]
	public PropertyBaseExtension Extension { get; set; }

	[JsonProperty]
	private DBAbstractions.Metamodel.Property _column;

	[JsonProperty]
	public string ID { get; set; }

	[JsonIgnore]
	public Type ClrType { get; set; }

	[JsonIgnore]
	public ValueType ValueType { get; set; }

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

	[JsonProperty]
	public bool IsPrimaryKey { get; set; }

	[JsonProperty]
	public ForeignKeyBase ForeignKey { get; set; }

	[JsonIgnore]
	public NavigationBase Navigation => ForeignKey?.DependentToPrincipal;

	[JsonIgnore]
	public bool? IsUnicode => _column.IsUnicode;

	[JsonProperty]
	public int? MaxLength { get; set; }

	[JsonProperty]
	public int? Precision { get; set; }

	[JsonProperty]
	public int? Scale { get; set; }

	[JsonProperty]
	public bool IsSingleUniqueConstraint { get; set; }

	[JsonIgnore]
	public bool? IsMultiUniqueConstraint => _column.IsMultiUniqueConstraint;

	[JsonIgnore]
	public bool IsIdentity => _column.IsIdentity;

	[JsonIgnore]
	public long? IdentityStart => _column.IdentityStart;

	[JsonIgnore]
	public long? IdentityIncrement => _column.IdentityIncrement;

	[JsonIgnore]
	public long? LastIdentity => _column.LastIdentity;

	[JsonProperty]
	public int PropertyOrdinal { get; set; }

	[JsonIgnore]
	public bool HasValueGeneratedMetohdName => _column.HasValueGeneratedMetohdName;

	[JsonIgnore]
	public string ValueGeneratedMetohdName => _column.ValueGeneratedMetohdName;

	[JsonIgnore]
	public ValueGenerated? ValueGenerated => _column.ValueGenerated;

	[JsonProperty]
	public bool IsConcurrencyToken { get; set; }

	[JsonProperty]
	public bool HasJsonConversion { get; set; }

	[JsonProperty]
	public List<IndexBase> Indexes { get; set; }

	[JsonProperty]
	public EntityBase DeclaringEntity { get; set; }

	[JsonIgnore]
	public List<string> Namespaces { get; set; }

	[JsonIgnore]
	public int ValidationsCount { get; set; }

	[JsonIgnore]
	public bool WithNotDefaultValidation { get; set; }

	[JsonIgnore]
	public bool WithMaxLengthValidation { get; set; }

	[JsonIgnore]
	public bool WithPrecisionScaleValidation { get; set; }

	[JsonIgnore]
	public bool WithMinDateTimeValidation { get; set; }




	[JsonIgnore]
	public bool IsRequiredEfMapping => !IsNullable && ClrType.IsNullable() && !IsPrimaryKey;

	[JsonIgnore]
	public bool IsExplicitName => ColumnName != null && ColumnName != Name;

	[JsonProperty]
	public bool IsIgnored { get; set; }


	public PropertyBase()
	{
		Indexes = new List<IndexBase>();
		Namespaces = new List<string>();
	}

	internal void Init(DBAbstractions.Metamodel.Property column, PropertyBase? ext)
	{
		_column = column ?? throw new ArgumentNullException(nameof(column));
		ID = _column.ColumnName;

		var type = _column.ClrType;
		if (type == typeof(char))
			type = typeof(string);
		if (type == typeof(char?))
			type = typeof(string);

		Namespaces = new List<string>(_column.Namespaces);

		Name = _column.Name;
		PropertyOrdinal = _column.ColumnOrdinal;
		IsNullable = _column.IsNullable;
		IsPrimaryKey = _column.IsPrimaryKey;
		MaxLength = _column.MaxLength;
		Precision = _column.Precision;
		Scale = _column.Scale;
		IsSingleUniqueConstraint = _column.IsSingleUniqueConstraint ?? false;
		IsConcurrencyToken = _column.IsConcurrencyToken;
		
		Extension = new PropertyBaseExtension();
		
		if (ext != null)
		{
			if (ext.Extension != null)
				Extension = ext.Extension;

			if (Extension.NameExtended && !string.IsNullOrWhiteSpace(ext.Name))
				Name = ext.Name;

			if (Extension.IsConcurrencyTokenExtended) IsConcurrencyToken = ext.IsConcurrencyToken;
			if (Extension.IsIgnoredExtended) IsIgnored = ext.IsIgnored;
			if (Extension.HasJsonConversionExtended) HasJsonConversion = ext.HasJsonConversion;
		}

		ClrType = HasJsonConversion
			? typeof(List<>).MakeGenericType(type)
			: type;

		var underlyingNullableType = ClrType.GetUnderlyingNullableType();

		UnderlyingNullableType = underlyingNullableType;
		CSharpType = TypeHelper.TypeToCSharpSourceCode(ClrType);
		ValueType = Helpers.ValueTypeHelper.GetValueType(ClrType);

		ValidationsCount = 0;

		if (IsRequiredEfMapping && !IsPrimaryKey && UnderlyingNullableType != typeof(bool))
		{
			WithNotDefaultValidation = true;
			ValidationsCount++;
		}

		if (0 < MaxLength)
		{
			WithMaxLengthValidation = true;
			ValidationsCount++;
		}

		if ((typeof(decimal) == UnderlyingNullableType || typeof(double) == UnderlyingNullableType || typeof(float) == UnderlyingNullableType) && (0 < Precision || 0 < Scale))
		{
			WithPrecisionScaleValidation = true;
			ValidationsCount++;
		}

		if (typeof(DateTime) == UnderlyingNullableType)
		{
			WithMinDateTimeValidation = true;
			ValidationsCount++;
		}
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
