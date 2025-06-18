namespace Legion.Database.Metamodel;

public interface IView
{
	string Name { get; }
	int? Id { get; }
	string Definition { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	string? Alias { get; }
	IEnumerable<IColumn> Columns { get; }

	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	ISchema Schema { get; }
}
