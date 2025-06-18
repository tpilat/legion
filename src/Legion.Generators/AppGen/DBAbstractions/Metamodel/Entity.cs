using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class Entity
{
	[JsonProperty]
	public string Schema { get; set; }

	[JsonProperty]
	public string TableName { get; set; }

	[JsonProperty]
	public bool IsDbView { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Key PrimaryKey { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Property> Properties { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Navigation> Navigations { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Navigation> BackNavigations { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Index> Indexes { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Key> Keys { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.ForeignKey> ForeignKeys { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Model Model { get; set; }

	public Entity()
	{
		Properties = new List<Property>();
		Navigations = new List<Navigation>();
		BackNavigations = new List<Navigation>();
		Indexes = new List<Index>();
		Keys = new List<Key>();
		ForeignKeys = new List<ForeignKey>();
	}

	public override string ToString()
	{
		return $"{Schema}.{TableName}";
	}
}
