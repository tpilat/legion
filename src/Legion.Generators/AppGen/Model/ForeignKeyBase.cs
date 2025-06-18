using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class ForeignKeyBase : ICommonBaseModel
{
	[JsonProperty]
	private DBAbstractions.Metamodel.ForeignKey _fk;

	[JsonIgnore]
	public string Name => _fk.Name;

	[JsonProperty]
	public NavigationBase DependentToPrincipal { get; set; }

	[JsonProperty]
	public BackNavigationBase PrincipalToDependent { get; set; }

	[JsonProperty]
	public EntityBase DeclaringEntity { get; set; }

	[JsonProperty]
	public EntityBase PrincipalEntity { get; set; }

	[JsonIgnore]
	public bool IsUnique => _fk.IsUnique;

	[JsonIgnore]
	public bool IsRequired => _fk.IsRequired;

	[JsonProperty]
	public KeyBase PrincipalKey { get; set; }

	[JsonIgnore]
	public DBAbstractions.Metamodel.DeleteBehavior DeleteBehavior => _fk.DeleteBehavior;

	[JsonProperty]
	public List<PropertyBase> Properties { get; set; }

	[JsonIgnore]
	public bool PrincipalKeyIsPrimaryKey => PrincipalKey.IsPrimaryKey;

	//[JsonIgnore]
	//public string PrincipalEntityDisplayName => PrincipalEntity.Name;

	//[JsonIgnore]
	//public string DeclaringEntityDisplayName => DeclaringEntity.Name;

	[JsonIgnore]
	public DBAbstractions.Metamodel.DeleteBehavior DefaultDeleteBehavior => IsRequired
														? DBAbstractions.Metamodel.DeleteBehavior.Cascade
														: DBAbstractions.Metamodel.DeleteBehavior.ClientSetNull;

	public ForeignKeyBase()
	{
		Properties = new List<PropertyBase>();
	}

	internal void Init(DBAbstractions.Metamodel.ForeignKey fk)
	{
		_fk = fk ?? throw new ArgumentNullException(nameof(fk));
	}

	public override string ToString()
	{
		return Name;
	}
}
