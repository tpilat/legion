using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class Navigation
{
	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public Type ClrType { get; set; }

	[JsonProperty]
	public bool IsCollection { get; set; }

	[JsonProperty]
	public bool IsDependentToPrincipal { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Entity DeclaringEntity { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.Entity TargetType { get; set; }

	[JsonProperty]
	public AppGen.DBAbstractions.Metamodel.ForeignKey ForeignKey { get; set; }

	public override string ToString()
	{
		return Name;
	}
}
