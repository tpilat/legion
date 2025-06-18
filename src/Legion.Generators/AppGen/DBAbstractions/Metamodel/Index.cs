using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class Index
{
	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public bool IsUnique { get; set; }

	[JsonProperty]
	public string Filter { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Entity DeclaringEntity { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Property> Properties { get; set; }

	public Index()
	{
		Properties = new List<Property>();
	}

	public override string ToString()
	{
		return Name;
	}
}
