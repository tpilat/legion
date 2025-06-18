using Newtonsoft.Json;

namespace Legion.Generators.AppGen.DBAbstractions.Metamodel;

[Serializable]
public class Model
{
	[JsonProperty]
	public string DefaultSchema { get; set; }

	[JsonProperty]
	public List<Entity> TableEntities { get; set; }

	[JsonProperty]
	public List<Entity> ViewEntities { get; set; }

	[JsonProperty]
	public List<Sequence> Sequences { get; set; }

	public Model()
	{
		TableEntities = new List<Entity>();
		ViewEntities = new List<Entity>();
		Sequences = new List<Sequence>();
	}

	public ModelResult Validate()
	{
		return Validate(null);
	}

	public ModelResult Validate(ModelResult modelResult)
	{
		if (modelResult == null)
			modelResult = new ModelResult();

		//TODO
		/*
DB Check:
- ak nazov stlpca zacina na Id / ID a nie je to FK ani PK tak vypis Warning
		*/

		return modelResult;
	}
}
