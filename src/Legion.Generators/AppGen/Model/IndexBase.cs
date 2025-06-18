using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class IndexBase : ICommonBaseModel
{
	[JsonProperty]
	private DBAbstractions.Metamodel.Index _idx;

	[JsonIgnore]
	public string Name => _idx.Name;

	[JsonIgnore]
	public bool IsUnique => _idx.IsUnique;

	[JsonIgnore]
	public string Filter => _idx.Filter;

	[JsonProperty]
	public EntityBase DeclaringEntity { get; set; }

	[JsonProperty]
	public List<PropertyBase> Properties { get; set; }

	public IndexBase()
	{
		Properties = new List<PropertyBase>();
	}

	internal void Init(DBAbstractions.Metamodel.Index idx)
	{
		_idx = idx ?? throw new ArgumentNullException(nameof(idx));
	}

	public override string ToString()
	{
		return Name;
	}
}
