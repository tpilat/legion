using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class Sequence
{
	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public string Schema { get; set; }

	[JsonProperty]
	public Model Model { get; set; }

	[JsonProperty]
	public Type ClrType { get; set; }

	[JsonProperty]
	public long StartValue { get; set; }

	[JsonProperty]
	public int IncrementBy { get; set; }

	[JsonProperty]
	public long? MinValue { get; set; }

	[JsonProperty]
	public long? MaxValue { get; set; }

	[JsonProperty]
	public bool IsCyclic { get; set; }

	public override string ToString()
	{
		return $"{Name}";
	}
}
