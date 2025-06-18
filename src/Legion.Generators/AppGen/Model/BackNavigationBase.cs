using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class BackNavigationBaseExtension
{
	[JsonProperty]
	public bool NameExtended { get; set; }

	[JsonProperty]
	public bool IgnoreExtended { get; set; }

	[JsonProperty]
	public bool IsCollectionExtended { get; set; }
}

[Serializable]
public class BackNavigationBase : ICommonBaseModel
{
	[JsonProperty]
	public BackNavigationBaseExtension Extension { get; set; }

	[JsonProperty]
	private DBAbstractions.Metamodel.Navigation _nav;

	[JsonProperty]
	public string ID { get; set; }

	[JsonProperty]
	public string Name { get; set; }

	[JsonIgnore]
	public string CSharpType => IsCollection
		? $"ICollection<{TargetType.Name}>"
		: TargetType.Name;

	[JsonIgnore]
	public Type ClrType => _nav.ClrType;

	[JsonIgnore]
	public bool IsDependentToPrincipal => _nav.IsDependentToPrincipal;

	[JsonProperty]
	public bool IsCollection { get; set; }

	[JsonProperty]
	public EntityBase DeclaringEntity { get; set; }

	[JsonProperty]
	public EntityBase TargetType { get; set; }

	[JsonProperty]
	public ForeignKeyBase ForeignKey { get; set; }

	[JsonIgnore]
	public NavigationBase Navigation => ForeignKey.DependentToPrincipal;

	[JsonProperty]
	public bool Ignore { get; set; }

	internal void Init(DBAbstractions.Metamodel.Navigation nav, BackNavigationBase? ext)
	{
		_nav = nav ?? throw new ArgumentNullException(nameof(nav));
		ID = _nav.Name;
		Name = nav.Name;
		IsCollection = _nav.IsCollection;
		Ignore = false;
		Extension = new BackNavigationBaseExtension();

		if (ext != null)
		{
			if (nav.DeclaringEntity.Schema != ext.DeclaringEntity.Schema
				|| nav.DeclaringEntity.TableName != ext.DeclaringEntity.TableName
				|| nav.TargetType.Schema != ext.TargetType.Schema
				|| nav.TargetType.TableName != ext.TargetType.TableName)
			{
				//throw new InvalidOperationException($"Mismatch extension {nav.DeclaringEntity}.{nav.Name}");
			}
			else
			{
				if (ext.Extension != null)
					Extension = ext.Extension;

				if (Extension.NameExtended && !string.IsNullOrWhiteSpace(ext.Name))
					Name = ext.Name;

				if (Extension.IgnoreExtended)
					Ignore = ext.Ignore;

				if (Extension.IsCollectionExtended)
					IsCollection = ext.IsCollection;
			}
		}
	}

	public void ResetName()
	{
		Name = _nav.Name;
	}

	public override string ToString()
	{
		return Name;
	}
}
