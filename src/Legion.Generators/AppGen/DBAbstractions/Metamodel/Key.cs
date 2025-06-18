using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class Key
{
	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public string DefaultName { get; set; }

	[JsonProperty]
	public bool IsPrimaryKey { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Entity DeclaringEntity { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Property> Properties { get; set; }

	public Key()
	{
		Properties = new List<Property>();
	}

	public override string ToString()
	{
		return Name;
	}
}
