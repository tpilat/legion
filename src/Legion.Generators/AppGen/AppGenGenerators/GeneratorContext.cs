using Legion.Database.Readers;
using Legion.Extensions;
using Legion.Generators.AppGen.Descriptors;
using Legion.Generators.AppGen.Helpers;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.AppGenGenerators;

public class GeneratorContext
{
	public ModelBase Model { get; }
	public ISqlReader SqlReader { get; }

	public EntityContextDescriptor EntityContextDescriptor { get; set; }
	public QueryContextDescriptor QueryContextDescriptor { get; set; }

	public Dictionary<string, BaseEntityDescriptor> BaseEntityDescriptors { get; set; }
	public Dictionary<string, RepositoryDescriptor> RepositoryDescriptors { get; set; }
	public Dictionary<string, TableInfoDescriptor> TableInfoDescriptors { get; set; }
	public Dictionary<string, BaseQueryEntityDescriptor> BaseQueryEntityDescriptors { get; set; }
	public Dictionary<string, QueryRepositoryDescriptor> QueryRepositoryDescriptors { get; set; }
	public Dictionary<string, QueryTableInfoDescriptor> QueryTableInfoDescriptors { get; set; }
	private Dictionary<EntityBase, EntityModelDescriptor> ModelEntityMap { get; }
	private Dictionary<EntityBase, EntityModelEnumDescriptor> ModelEntityEnumMap { get; }
	private Dictionary<EntityBase, EntityModelEnumerationDescriptor> ModelEntityEnumerationMap { get; }
	private Dictionary<QueryEntityBase, QueryModelDescriptor> QueryModelQueryEntityMap { get; }

	public GeneratorContext(ModelBase model)
	{
		Model = model ?? throw new ArgumentNullException(nameof(model));
		SqlReader = SqlHelper.Create(CodeGeneratorConfig.Instance.SelectedDatabaseConnection.GetConnectionString(), CodeGeneratorConfig.Instance.SelectedDatabaseConnection.Provider);

		ModelEntityMap = new Dictionary<EntityBase, EntityModelDescriptor>();
		ModelEntityEnumMap = new Dictionary<EntityBase, EntityModelEnumDescriptor>();
		ModelEntityEnumerationMap = new Dictionary<EntityBase, EntityModelEnumerationDescriptor>();
		QueryModelQueryEntityMap = new Dictionary<QueryEntityBase, QueryModelDescriptor>();
	}

	internal void BuildDescriptors(ModelResult modelResult)
	{
		BaseEntityDescriptors = [];
		BaseQueryEntityDescriptors = [];
		RepositoryDescriptors = [];
		QueryRepositoryDescriptors = [];
		TableInfoDescriptors = [];
		QueryTableInfoDescriptors = [];

		foreach (var entity in Model.Entities)
		{
			if (string.IsNullOrWhiteSpace(entity.CustomBaseEntityName))
				BaseEntityDescriptors.AddUniqueKey(entity.BaseEntityName, new BaseEntityDescriptor(entity.BaseEntityName, Model, this));

			RepositoryDescriptors.AddUniqueKey(entity.RepositoryName, new RepositoryDescriptor(entity.RepositoryName, Model, this));
			TableInfoDescriptors.AddUniqueKey(entity.RepositoryName, new TableInfoDescriptor(Model, this));

			ModelEntityMap.Add(entity, new EntityModelDescriptor(entity, this));
			if (entity.ConvertEntity == ConvertEntity.ToEnum
				|| entity.ConvertEntity == ConvertEntity.ToEnumAndEnumerationClass)
				ModelEntityEnumMap.Add(entity, new EntityModelEnumDescriptor(entity, this));

			if (entity.ConvertEntity == ConvertEntity.ToEnumerationClass
				|| entity.ConvertEntity == ConvertEntity.ToEnumAndEnumerationClass)
				ModelEntityEnumerationMap.Add(entity, new EntityModelEnumerationDescriptor(entity, this));
		}

		foreach (var queryEntity in Model.QueryEntities)
		{
			BaseQueryEntityDescriptors.AddUniqueKey(queryEntity.BaseQueryEntityName, new BaseQueryEntityDescriptor(queryEntity.BaseQueryEntityName, Model, this));
			QueryRepositoryDescriptors.AddUniqueKey(queryEntity.QueryRepositoryName, new QueryRepositoryDescriptor(queryEntity.QueryRepositoryName, Model, this));
			QueryTableInfoDescriptors.AddUniqueKey(queryEntity.QueryRepositoryName, new QueryTableInfoDescriptor(Model, this));
			QueryModelQueryEntityMap.Add(queryEntity, new QueryModelDescriptor(queryEntity, this));
		}

		EntityContextDescriptor = new EntityContextDescriptor(Model, this);
		QueryContextDescriptor = new QueryContextDescriptor(Model, this);

		CheckValidate(modelResult);
	}

	private void CheckValidate(ModelResult modelResult)
	{
		var entityDbSetNames = new Dictionary<string, EntityBase>();
		var queryEntityDbSetNames = new Dictionary<string, QueryEntityBase>();
		var uniqueNames = new Dictionary<string, string>();
		List<string> auditEntries = new List<string>();

		foreach (var entity in Model.Entities)
		{
			if (uniqueNames.TryGetValue(entity.UniqueName, out string id))
				modelResult.AddError("EntityModel", $"{entity.FullName} {nameof(entity.UniqueName)} = {entity.UniqueName} must be unique. Conflict with {id}");
			else
				uniqueNames.Add(entity.UniqueName, entity.ID);

			if (entity.IsAuditEntry)
				auditEntries.Add(entity.Name);
		}

		if (1 < auditEntries.Count)
			modelResult.AddError("EntityModel", $"More than 1 IAuditEntry found: {string.Join(", ", auditEntries)}");

		foreach (var queryEntity in Model.QueryEntities)
		{
			if (uniqueNames.TryGetValue(queryEntity.UniqueName, out string id))
				modelResult.AddError("QueryModel", $"{queryEntity.FullName} {nameof(queryEntity.UniqueName)} = {queryEntity.UniqueName} must be unique. Conflict with {id}");
			else
				uniqueNames.Add(queryEntity.UniqueName, queryEntity.ID);
		}

		foreach (var entity in Model.Entities)
		{
			//var pkg = entity.Package;
			//while (pkg != null)
			//{
			//	if (entity.Name == pkg.Name)
			//		modelResult.AddError("EntityModel", $"{entity.FullName} cannot be equal to it's package (parent package) name");

			//	pkg = pkg.Parent;
			//}

			if (entityDbSetNames.TryGetValue(entity.DbSetName, out EntityBase ent))
				modelResult.AddError("EntityModel", $"{entity.FullName} {nameof(entity.DbSetName)} = {entity.DbSetName} must be unique. Conflict between {ent.ID} and {entity.ID}");
			else
				entityDbSetNames.Add(entity.DbSetName, entity);
		}

		foreach (var queryEntity in Model.QueryEntities)
		{
			//var pkg = queryEntity.Package;
			//while (pkg != null)
			//{
			//	if (queryEntity.Name == pkg.Name)
			//		modelResult.AddError("QueryModel", $"{queryEntity.FullName} cannot be equal to it's package (parent package) name");

			//	pkg = pkg.Parent;
			//}

			if (queryEntityDbSetNames.TryGetValue(queryEntity.DbSetName, out QueryEntityBase ent))
				modelResult.AddError("QueryModel", $"{queryEntity.FullName} {nameof(queryEntity.DbSetName)} = {queryEntity.DbSetName} must be unique. Conflict between {ent.ID} and {queryEntity.ID}");
			else
				queryEntityDbSetNames.Add(queryEntity.DbSetName, queryEntity);
		}
	}

	public EntityModelDescriptor GetEntityModelDescriptor(EntityBase entity, bool onlyIfCanGenerate, string modelName, string contextName)
	{
		if (onlyIfCanGenerate == false || entity.GenerateModel)
		{
			var result = ModelEntityMap[entity];

			if (result.Data.ModelContextNames.ContainsKey(modelName))
			{
				result.Initialize(modelName, contextName);
				return result;
			}
		}

		return null;
	}

	public IReadOnlyCollection<EntityModelDescriptor> GetAllEntityModelDescriptor(string modelName, string contextName)
		=> ModelEntityMap.Values.Where(x => x.Data.GenerateModel && x.Data.ModelContextNames.ContainsKey(modelName)).ToList();

	public IReadOnlyCollection<EntityModelEnumDescriptor> GetAllEntityModelEnumDescriptor(string modelName, string contextName)
		=> ModelEntityEnumMap.Values.Where(x => x.Data.GenerateModel && x.Data.ModelContextNames.ContainsKey(modelName)).ToList();

	public IReadOnlyCollection<EntityModelEnumerationDescriptor> GetAllEntityModelEnumerationDescriptor(string modelName, string contextName)
		=> ModelEntityEnumerationMap.Values.Where(x => x.Data.GenerateModel && x.Data.ModelContextNames.ContainsKey(modelName)).ToList();

	public QueryModelDescriptor GetQueryModelDescriptor(QueryEntityBase queryEntity, string modelName, string contextName)
	{
		if (queryEntity.GenerateQueryModel)
		{
			var result = QueryModelQueryEntityMap[queryEntity];

			if (result.Data.QueryModelContextNames.ContainsKey(modelName))
			{
				result.Initialize(modelName, contextName);
				return result;
			}
		}

		return null;
	}

	public IReadOnlyCollection<QueryModelDescriptor> GetAllQueryModelDescriptor(string modelName, string contextName)
		=> QueryModelQueryEntityMap.Values.Where(x => x.Data.GenerateQueryModel && x.Data.QueryModelContextNames.ContainsKey(modelName)).ToList();
}
