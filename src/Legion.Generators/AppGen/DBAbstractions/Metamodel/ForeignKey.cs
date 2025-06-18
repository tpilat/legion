using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class ForeignKey
{
	[JsonProperty]
	public string Schema { get; set; }

	[JsonProperty]
	public string TableName { get; set; }

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Navigation DependentToPrincipal { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Navigation PrincipalToDependent { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Entity DeclaringEntity { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Entity PrincipalEntity { get; set; }

	[JsonProperty]
	public bool IsUnique { get; set; }

	[JsonProperty]
	public bool IsRequired { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Key PrincipalKey { get; set; }

	[JsonProperty]
	public DeleteBehavior DeleteBehavior { get; set; }

	[JsonProperty]
	public List<AppGen.DBAbstractions.Metamodel.Property> Properties { get; set; }

	public ForeignKey()
	{
		Properties = new List<Property>();
	}

	public override string ToString()
	{
		return Name;
	}
}
