using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class KeyBase : ICommonBaseModel
{
	[JsonProperty]
	private DBAbstractions.Metamodel.Key _key;

	[JsonIgnore]
	public string Name => _key.Name;

	[JsonIgnore]
	public string DefaultName => _key.DefaultName;

	[JsonIgnore]
	public bool IsPrimaryKey => _key.IsPrimaryKey;

	[JsonProperty]
	public EntityBase DeclaringEntity { get; set; }

	[JsonProperty]
	public List<PropertyBase> Properties { get; set; }

	[JsonProperty]
	public bool ModelCreating_HasKey { get; set; }

	public KeyBase()
	{
		Properties = new List<PropertyBase>();
	}

	internal void Init(DBAbstractions.Metamodel.Key key)
	{
		_key = key ?? throw new ArgumentNullException(nameof(key));
		ModelCreating_HasKey = true;
	}

	public override string ToString()
	{
		return Name;
	}
}
