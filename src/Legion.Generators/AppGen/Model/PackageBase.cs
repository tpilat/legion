using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class PackageBaseExtension
{
	[JsonProperty]
	public bool NameExtended { get; set; }
}

[Serializable]
public class PackageBase : ICommonBaseModel
{
	[JsonProperty]
	public PackageBaseExtension Extension { get; set; }

	[JsonProperty]
	public string ID { get; set; }

	[JsonProperty]
	public string Schema { get; set; }

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public PackageBase Parent { get; set; }

	[JsonProperty]
	public List<PackageBase> ChildPackages { get; set; }

	[JsonProperty]
	public List<EntityBase> Entities { get; set; }

	[JsonProperty]
	public List<QueryEntityBase> QueryEntities { get; set; }

	[JsonProperty]
	public ModelBase Model { get; set; }

	[JsonIgnore]
	public string NamespacePart => string.Join(".", GetPackagesStructure().Select(p => p.Name));

	[JsonIgnore]
	public string PathPart => string.Join("\\", GetPackagesStructure().Select(p => p.Name));

	public PackageBase()
	{
		ChildPackages = new List<PackageBase>();
		Entities = new List<EntityBase>();
		QueryEntities = new List<QueryEntityBase>();
	}

	internal void Init(string schema, PackageBase? ext)
	{
		if (string.IsNullOrWhiteSpace(schema))
			throw new ArgumentNullException(nameof(schema));

		ID = schema;
		Schema = schema;
		Name = schema;
		Extension = new PackageBaseExtension();

		if (ext != null)
		{
			if (ext.Extension != null)
				Extension = ext.Extension;

			if (Extension.NameExtended && !string.IsNullOrWhiteSpace(ext.Name))
				Name = ext.Name;
		}
	}

	public override string ToString()
	{
		if (string.Equals(Name, Schema, StringComparison.OrdinalIgnoreCase))
			return Name;
		else
			return $"{Name} ({Schema})";
	}

	public List<PackageBase> GetPackagesStructure()
	{
		return GetPackagesStructureInternal(new List<PackageBase>());
	}

	private List<PackageBase> GetPackagesStructureInternal(List<PackageBase> path)
	{
		path.Insert(0, this);
		Parent?.GetPackagesStructureInternal(path);

		return path;
	}
}
