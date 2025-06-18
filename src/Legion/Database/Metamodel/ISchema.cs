namespace Legion.Database.Metamodel;

public interface ISchema
{
	[Newtonsoft.Json.JsonIgnore]
#if NET6_0_OR_GREATER
	[System.Text.Json.Serialization.JsonIgnore]
#endif
	[System.Xml.Serialization.XmlIgnore]
	IDatabaseModel Model { get; }

	int? Id { get; }
	string Name { get; }
	string Alias { get; }
	IEnumerable<ITable>? Tables { get; }
	IEnumerable<IView>? Views { get; }
	List<Internal.Sequence>? Sequences { get; set; }
}
