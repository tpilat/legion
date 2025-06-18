using Legion.Generators.AppGen.Model.Config;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class ModelBase : ICommonBaseModel
{
	[JsonProperty]
	public CodeGeneratorSettings Settings { get; set; }

	[JsonProperty]
	public string DefaultSchema { get; set; }

	[JsonProperty]
	public List<PackageBase> Packages { get; set; }

	[JsonProperty]
	public List<EntityBase> Entities { get; set; }

	[JsonProperty]
	public List<QueryEntityBase> QueryEntities { get; set; }

	[JsonProperty]
	public List<SequenceBase> Sequences { get; set; }

	[JsonProperty]
	public Dictionary<string, string> ModelContextDict { get; set; }

	[JsonProperty]
	public Dictionary<string, List<EntityBase>> ModelEntitiesDict { get; set; }

	[JsonProperty]
	public Dictionary<string, string> QueryModelContextDict { get; set; }

	[JsonProperty]
	public Dictionary<string, List<QueryEntityBase>> QueryModelEntitiesDict { get; set; }

	public ModelBase()
	{
		Settings = new CodeGeneratorSettings();
		Packages = new List<PackageBase>();
		Entities = new List<EntityBase>();
		QueryEntities = new List<QueryEntityBase>();
		Sequences = new List<SequenceBase>();
		ModelContextDict = new Dictionary<string, string>();
		ModelEntitiesDict = new Dictionary<string, List<EntityBase>>();
		QueryModelContextDict = new Dictionary<string, string>();
		QueryModelEntitiesDict = new Dictionary<string, List<QueryEntityBase>>();
	}

	public PackageBase GetPackage(string id)
	{
		return Packages.FirstOrDefault(x => x.ID == id);
	}

	public EntityBase GetEntity(string id)
	{
		return Entities.FirstOrDefault(x => x.ID == id);
	}

	public QueryEntityBase GetQueryEntity(string id)
	{
		return QueryEntities.FirstOrDefault(x => x.ID == id);
	}

	public void ResetAllNames()
	{
		foreach (var entity in Entities)
		{
			entity.ResetName();

			entity.ResetPropertyNames();
			entity.ResetNavigationNames();
			entity.ResetBackNavigationNames();
		}

		foreach (var queryEntity in QueryEntities)
		{
			queryEntity.ResetName();

			queryEntity.ResetPropertyNames();
		}
	}
}
