namespace Legion.Database.Metamodel;

public interface ITable
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	ISchema Schema { get; }

	int? Id { get; }
	string Name { get; }
	string Alias { get; }
	IEnumerable<IColumn> Columns { get; }
	IPrimaryKey? PrimaryKey { get; }
	IEnumerable<IForeignKey>? ForeignKeys { get; }
	IEnumerable<IUniqueConstraint>? UniqueConstraints { get; }
	IEnumerable<IIndex>? Indexes { get; }
}
