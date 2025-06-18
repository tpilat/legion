using Legion.Extensions;
using Legion.Text;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class QueryEntityBaseExtension
{
	[JsonProperty]
	public bool ModelNamesExtended { get; set; }

	[JsonProperty]
	public bool ContextNamesExtended { get; set; }

	[JsonProperty]
	public bool NameExtended { get; set; }

	[JsonProperty]
	public bool BaseQueryEntityNameExtended { get; set; }

	[JsonProperty]
	public bool CustomBaseQueryEntityNameExtended { get; set; }

	[JsonProperty]
	public bool IsSealedExtended { get; set; }

	[JsonProperty]
	public bool QueryRepositoryNameExtended { get; set; }

	[JsonProperty]
	public bool UniqueNameExtended { get; set; }

	[JsonProperty]
	public bool SingularizedNameExtended { get; set; }

	[JsonProperty]
	public bool DbSetNameExtended { get; set; }

	[JsonProperty]
	public bool GenerateQueryModelExtended { get; set; }

	[JsonProperty]
	public bool ActivityTokenExtended { get; set; }

	[JsonProperty]
	public bool CustomInterfacesExtended { get; set; }
}

[Serializable]
public class QueryEntityBase : ICommonBaseModel, IEntity
{
	[JsonProperty]
	public QueryEntityBaseExtension Extension { get; set; }

	[JsonProperty]
	private DBAbstractions.Metamodel.Entity _view;

	[JsonProperty]
	public string ID { get; set; }

	[JsonIgnore]
	public string FullName => Package == null
		? Name
		: $"{Package.NamespacePart}.{Name}";

	[JsonIgnore]
	public int EntityId { get; set; }

	[JsonIgnore]
	public string Schema => _view.Schema;

	[JsonIgnore]
	public string TableName => _view.TableName;

	[JsonProperty]
	public string ModelNames { get; set; }

	[JsonProperty]
	public string ContextNames { get; set; }

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public string BaseQueryEntityName { get; set; }

	[JsonProperty]
	public string CustomBaseQueryEntityName { get; set; }

	[JsonProperty]
	public bool IsSealed { get; set; }

	[JsonProperty]
	public string QueryRepositoryName { get; set; }

	[JsonProperty]
	public string UniqueName { get; set; }

	[JsonIgnore]
	public bool IsDbView => true;

	[JsonProperty]
	public List<QueryPropertyBase> Properties { get; set; }

	[JsonIgnore]
	public QueryPropertyBase FirstProperty => Properties[0];

	[JsonProperty]
	public ModelBase Model { get; set; }

	[JsonProperty]
	public PackageBase Package { get; set; }

	[JsonIgnore]
	public bool IsExplicitSchemaName => Schema != null && Schema != Model.DefaultSchema;

	[JsonIgnore]
	public bool IsExplicitObjectName => IsExplicitSchemaName || TableName != null && TableName != DbSetName;

	[JsonProperty]
	public string SingularizedName { get; set; }

	[JsonProperty]
	public string DbSetName { get; set; }

	[JsonProperty]
	public bool GenerateQueryModel { get; set; }

	[JsonProperty]
	public string ActivityToken { get; set; }

	[JsonProperty]
	public QueryPropertyBase MainColumn { get; set; }

	[JsonProperty]
	public string CustomInterfaces { get; set; }


	[JsonIgnore]
	public string AsCommandPrefix => $"{string.Join("_", Package.GetPackagesStructure().Select(p => p.Name))}_{Name}";

	[JsonIgnore]
	public Dictionary<string, string> QueryModelContextNames { get; set; }

	public QueryEntityBase()
	{
		Properties = new List<QueryPropertyBase>();
	}

	public string ToActivityName(string token)
	{
		if ("Activity".Equals(token, StringComparison.OrdinalIgnoreCase))
			token = token + "_";

		return token;
	}

	public string GetActivityTokenName()
		=> ToActivityName(UniqueName);

	internal void Init(DBAbstractions.Metamodel.Entity view, QueryEntityBase? ext)
	{
		_view = view ?? throw new ArgumentNullException(nameof(view));
		ID = $"{_view.Schema}.{_view.TableName}";

		var name = TableName.ToCammelCase();
		ModelNames = "Model";
		ContextNames = "QueryDbContext";
		Name = name;
		BaseQueryEntityName = "QueryEntityBase";
		CustomBaseQueryEntityName = null;
		IsSealed = true;
		QueryRepositoryName = "QueryRepository";
		UniqueName = name;
		SingularizedName = Name.Singularize();
		DbSetName = Name;
		ActivityToken = GetActivityTokenName();
		GenerateQueryModel = true;
		Extension = new QueryEntityBaseExtension();
		CustomInterfaces = null;

		if (ext != null)
		{
			if (view.Schema != ext.Schema
				|| view.TableName != ext.TableName
				|| view.IsDbView != ext.IsDbView)
			{
				throw new InvalidOperationException($"Mismatch extension {view}");
			}

			if (ext.Extension != null)
				Extension = ext.Extension;

			if (Extension.NameExtended && !string.IsNullOrWhiteSpace(ext.Name))
				Name = ext.Name;

			if (Extension.BaseQueryEntityNameExtended && !string.IsNullOrWhiteSpace(ext.BaseQueryEntityName))
				BaseQueryEntityName = ext.BaseQueryEntityName;

			if (Extension.CustomBaseQueryEntityNameExtended && !string.IsNullOrWhiteSpace(ext.CustomBaseQueryEntityName))
				CustomBaseQueryEntityName = ext.CustomBaseQueryEntityName;

			if (Extension.IsSealedExtended)
				IsSealed = ext.IsSealed;

			if (Extension.QueryRepositoryNameExtended && !string.IsNullOrWhiteSpace(ext.QueryRepositoryName))
				QueryRepositoryName = ext.QueryRepositoryName;

			if (Extension.UniqueNameExtended && !string.IsNullOrWhiteSpace(ext.UniqueName))
				UniqueName = ext.UniqueName;

			if (Extension.SingularizedNameExtended && !string.IsNullOrWhiteSpace(ext.SingularizedName))
				SingularizedName = ext.SingularizedName;

			if (Extension.ActivityTokenExtended && !string.IsNullOrWhiteSpace(ext.ActivityToken))
				ActivityToken = ext.ActivityToken;

			if (Extension.DbSetNameExtended && !string.IsNullOrWhiteSpace(ext.DbSetName))
				DbSetName = ext.DbSetName;

			if (Extension.ModelNamesExtended) ModelNames = ext.ModelNames;
			if (Extension.ContextNamesExtended) ContextNames = ext.ContextNames;
			if (Extension.GenerateQueryModelExtended) GenerateQueryModel = ext.GenerateQueryModel;
			if (Extension.CustomInterfacesExtended) CustomInterfaces = ext.CustomInterfaces;
		}
	}

	private bool buildFinished = false;
	public void Build()
	{
		if (buildFinished)
			throw new InvalidOperationException(nameof(buildFinished));

		buildFinished = true;

		if (string.IsNullOrWhiteSpace(ModelNames))
			throw new InvalidOperationException($"{ID}: {nameof(ModelNames)} == null");

		if (string.IsNullOrWhiteSpace(ContextNames))
			throw new InvalidOperationException($"{ID}: {nameof(ContextNames)} == null");

		QueryModelContextNames = new Dictionary<string, string>();

		var modelNamesSplit = ModelNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		var contextNamesSplit = ContextNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

		if (modelNamesSplit.Length != contextNamesSplit.Length)
			throw new InvalidOperationException($"{ID}: {nameof(ModelNames)} and {nameof(ContextNames)} counts | {modelNamesSplit.Length} vs {contextNamesSplit.Length}");

		for (int i = 0; i < modelNamesSplit.Length; i++)
		{
			var modelName = modelNamesSplit[i]?.Trim();
			var contextName = contextNamesSplit[i]?.Trim();

			if (string.IsNullOrWhiteSpace(modelName))
				throw new InvalidOperationException($"{ID}: {nameof(ModelNames)}[{i}] == null");

			if (string.IsNullOrWhiteSpace(contextName))
				throw new InvalidOperationException($"{ID}: {nameof(ContextNames)}[{i}] == null");

			if (QueryModelContextNames.ContainsKey(modelName))
				throw new InvalidOperationException($"{ID}: Multiple {nameof(ModelNames)} - {modelName}");

			if (QueryModelContextNames.ContainsValue(contextName))
				throw new InvalidOperationException($"{ID}: Multiple {nameof(ContextNames)} - {contextName}");

			QueryModelContextNames.Add(modelName, contextName);
		}

		if (QueryModelContextNames.Count == 0)
			throw new InvalidOperationException($"{ID}: {nameof(QueryModelContextNames)} is empty");
	}

	public QueryPropertyBase GetQueryProperty(string id)
	{
		return Properties.FirstOrDefault(x => x.ID == id);
	}

	public void ResetName()
	{
		var name = TableName.ToCammelCase();
		Name = name;
		UniqueName = name;
	}

	public void ResetPropertyNames()
	{
		foreach (var queryProperty in Properties)
			queryProperty.ResetName();
	}

	public override string ToString()
	{
		return $"{Schema}.{TableName}";
	}
}
