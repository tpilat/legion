using Legion.Database.Internal;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class Property
{
	[JsonProperty]
	public Type ClrType { get; set; }

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public string ColumnName { get; set; }

	[JsonProperty]
	public string ColumnType { get; set; }

	[JsonProperty]
	public object DefaultValue { get; set; }

	[JsonProperty]
	public string DefaultValueSql { get; set; }

	[JsonProperty]
	public string ComputedColumnSql { get; set; }

	[JsonProperty]
	public string ConfiguredColumnType { get; set; }

	[JsonProperty]
	public bool IsNullable { get; set; }

	[JsonProperty]
	public bool IsPrimaryKey { get; set; }

	[JsonProperty]
	public bool? IsUnicode { get; set; }

	[JsonProperty]
	public int? MaxLength { get; set; }

	[JsonProperty]
	public int? Precision { get; set; }

	[JsonProperty]
	public int? Scale { get; set; }

	[JsonProperty]
	public bool? IsSingleUniqueConstraint { get; set; }

	[JsonProperty]
	public bool? IsMultiUniqueConstraint { get; set; }

	[JsonProperty]
	public bool IsIdentity { get; set; }

	[JsonProperty]
	public long? IdentityStart { get; set; }

	[JsonProperty]
	public long? IdentityIncrement { get; set; }

	[JsonProperty]
	public long? LastIdentity { get; set; }

	[JsonProperty]
	public int ColumnOrdinal { get; set; }

	[JsonProperty]
	public bool HasValueGeneratedMetohdName { get; set; }

	[JsonProperty]
	public string ValueGeneratedMetohdName { get; set; }

	[JsonProperty]
	public ValueGenerated? ValueGenerated { get; set; }

	[JsonProperty]
	public bool IsConcurrencyToken { get; set; }

	[JsonProperty]
	public bool HasJsonConversion { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Index> Indexes { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Entity DeclaringEntity { get; set; }

	[JsonProperty]
	public List<string> Namespaces { get; set; }

	public Property()
	{
		Indexes = new List<Index>();
		Namespaces = new List<string>();
	}

	public override string ToString()
	{
		return ColumnName;
	}
}
