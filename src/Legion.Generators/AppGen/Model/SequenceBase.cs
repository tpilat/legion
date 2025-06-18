using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class SequenceBase : ICommonBaseModel
{
	[JsonProperty]
	private DBAbstractions.Metamodel.Sequence _sequence;

	[JsonIgnore]
	public string Name => _sequence.Name;

	[JsonIgnore]
	public string Schema => _sequence.Schema;

	[JsonProperty]
	public ModelBase Model { get; set; }

	[JsonIgnore]
	public Type ClrType => _sequence.ClrType;

	[JsonIgnore]
	public long StartValue => _sequence.StartValue;

	[JsonIgnore]
	public int IncrementBy => _sequence.IncrementBy;

	[JsonIgnore]
	public long? MinValue => _sequence.MinValue;

	[JsonIgnore]
	public long? MaxValue => _sequence.MaxValue;

	[JsonIgnore]
	public bool IsCyclic => _sequence.IsCyclic;

	[JsonIgnore]
	public bool IsExplicitSchema => !string.IsNullOrEmpty(Schema) && Model.DefaultSchema != Schema;

	public SequenceBase()
	{
	}

	internal void Init(DBAbstractions.Metamodel.Sequence sequence)
	{
		_sequence = sequence ?? throw new ArgumentNullException(nameof(sequence));
	}

	public override string ToString()
	{
		return $"{Name}";
	}
}
