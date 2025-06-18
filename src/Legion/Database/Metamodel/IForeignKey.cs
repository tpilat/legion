namespace Legion.Database.Metamodel;

public interface IForeignKey
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	ITable Table { get; }

	string Name { get; }
	string Column { get; }
	string ForeignSchemaAlias { get; }
	string ForeignTableName { get; }
	string ForeignColumnName { get; }
	ReferentialAction? OnUpdateAction { get; }
	ReferentialAction? OnDeleteAction { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IColumn FromColumn { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IColumn ToColumn { get; }
}
