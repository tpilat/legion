using Legion.Extensions;
using Legion.Text;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model;

[Serializable]
public class EntityBaseExtension
{
	[JsonProperty]
	public bool ModelNamesExtended { get; set; }

	[JsonProperty]
	public bool ContextNamesExtended { get; set; }

	[JsonProperty]
	public bool NameExtended { get; set; }

	[JsonProperty]
	public bool BaseEntityNameExtended { get; set; }

	[JsonProperty]
	public bool CustomBaseEntityNameExtended { get; set; }

	[JsonProperty]
	public bool IsSealedExtended { get; set; }

	[JsonProperty]
	public bool RepositoryNameExtended { get; set; }

	[JsonProperty]
	public bool UniqueNameExtended { get; set; }

	[JsonProperty]
	public bool PluralizedNameExtended { get; set; }

	[JsonProperty]
	public bool DbSetNameExtended { get; set; }

	[JsonProperty]
	public bool GenerateModelExtended { get; set; }

	[JsonProperty]
	public bool ConvertEntityExtended { get; set; }

	[JsonProperty]
	public bool DbContextTypeExtended { get; set; }

	[JsonProperty]
	public bool IsAuditEntryExtended { get; set; }

	[JsonProperty]
	public bool IsSelfAuditableEntityExtended { get; set; }

	[JsonProperty]
	public bool IsAuditableEntityExtended { get; set; }

	[JsonProperty]
	public bool IsSynchronizableEntityExtended { get; set; }

	[JsonProperty]
	public bool IsCorrelableEntityExtended { get; set; }

	[JsonProperty]
	public bool CustomInterfacesExtended { get; set; }

	[JsonProperty]
	public bool ActivityTokenExtended { get; set; }
}

[Serializable]
public class EntityBase : ICommonBaseModel, IEntity
{
	[JsonProperty]
	public EntityBaseExtension Extension { get; set; }

	[JsonProperty]
	private DBAbstractions.Metamodel.Entity _table;

	[JsonProperty]
	public string ID { get; set; }

	[JsonIgnore]
	public string FullName => Package == null
		? Name
		: $"{Package.NamespacePart}.{Name}";

	[JsonIgnore]
	public int EntityId { get; set; }

	[JsonIgnore]
	public string Schema => _table.Schema;

	[JsonIgnore]
	public string TableName => _table.TableName;

	[JsonProperty]
	public string ModelNames { get; set; }

	[JsonProperty]
	public string ContextNames { get; set; }

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public string BaseEntityName { get; set; }

	[JsonProperty]
	public string CustomBaseEntityName { get; set; }

	[JsonProperty]
	public bool IsSealed { get; set; }

	[JsonProperty]
	public string RepositoryName { get; set; }

	[JsonProperty]
	public string UniqueName { get; set; }

	[JsonIgnore]
	public bool IsDbView => false;

	[JsonProperty]
	public KeyBase PrimaryKey { get; set; }

	[JsonIgnore]
	public PropertyBase FirstPrimaryKey => PrimaryKey?.Properties?[0];

	[JsonProperty]
	public List<PropertyBase> Properties { get; set; }

	[JsonProperty]
	public List<NavigationBase> Navigations { get; set; }

	[JsonProperty]
	public List<BackNavigationBase> BackNavigations { get; set; }

	[JsonProperty]
	public List<IndexBase> Indexes { get; set; }

	[JsonProperty]
	public List<KeyBase> Keys { get; set; }

	[JsonProperty]
	public List<ForeignKeyBase> ForeignKeys { get; set; }

	[JsonProperty]
	public ModelBase Model { get; set; }

	[JsonProperty]
	public PackageBase Package { get; set; }



	[JsonIgnore]
	public bool IsExplicitSchemaName => Schema != null && Schema != Model.DefaultSchema;

	[JsonIgnore]
	public bool IsExplicitObjectName => IsExplicitSchemaName || TableName != null && TableName != DbSetName;


	[JsonProperty]
	public string PluralizedName { get; set; }

	[JsonProperty]
	public string DbSetName { get; set; }

	[JsonProperty]
	public bool GenerateModel { get; set; }

	[JsonProperty]
	public ConvertEntity ConvertEntity { get; set; }

	[JsonProperty]
	public DbContextType DbContextType { get; set; }

	[JsonProperty]
	public PropertyBase MainColumn { get; set; }

	[JsonProperty]
	public bool IsAuditEntry { get; set; }

	[JsonProperty]
	public bool IsSelfAuditableEntity { get; set; }

	[JsonProperty]
	public bool IsAuditableEntity { get; set; }

	[JsonProperty]
	public bool IsSynchronizableEntity { get; set; }

	[JsonProperty]
	public bool IsCorrelableEntity { get; set; }

	[JsonProperty]
	public string CustomInterfaces { get; set; }

	[JsonProperty]
	public string ActivityToken { get; set; }


	[JsonIgnore]
	public bool IsAuditEntryEntity => Model.Settings.AuditEntryId == ID;

	[JsonIgnore]
	public string AsCommandPrefix => $"{string.Join("_", Package.GetPackagesStructure().Select(p => p.Name))}_{Name}";

	[JsonIgnore]
	public Dictionary<string, string> ModelContextNames { get; set; }

	public EntityBase()
	{
		Properties = new List<PropertyBase>();
		Navigations = new List<NavigationBase>();
		BackNavigations = new List<BackNavigationBase>();
		Indexes = new List<IndexBase>();
		Keys = new List<KeyBase>();
		ForeignKeys = new List<ForeignKeyBase>();
	}

	public string ToActivityName(string token)
	{
		if ("Activity".Equals(token, StringComparison.OrdinalIgnoreCase))
			token = token + "_";

		return token;
	}

	public string GetActivityTokenName()
		=> ToActivityName(UniqueName);

	internal void Init(DBAbstractions.Metamodel.Entity table, EntityBase? ext)
	{
		_table = table ?? throw new ArgumentNullException(nameof(table));
		ID = $"{_table.Schema}.{_table.TableName}";

		var name = TableName.ToCammelCase();
		ModelNames = "Model";
		ContextNames = "EntityDbContext";
		Name = name;
		BaseEntityName = "EntityBase";
		CustomBaseEntityName = null;
		IsSealed = true;
		RepositoryName = "Repository";
		UniqueName = name;
		PluralizedName = name.Pluralize();
		DbSetName = Name;
		GenerateModel = true;
		ConvertEntity = ConvertEntity.None;
		DbContextType = DbContextType.DbSet;
		ActivityToken = GetActivityTokenName();
		IsAuditEntry = false;
		IsSelfAuditableEntity = false;
		IsAuditableEntity = false;
		IsSynchronizableEntity = false;
		IsCorrelableEntity = false;
		CustomInterfaces = null;
		Extension = new EntityBaseExtension();

		if (ext != null)
		{
			if (table.Schema != ext.Schema
				|| table.TableName != ext.TableName
				|| table.IsDbView != ext.IsDbView)
			{
				throw new InvalidOperationException($"Mismatch extension {table}");
			}

			if (ext.Extension != null)
				Extension = ext.Extension;

			if (Extension.NameExtended && !string.IsNullOrWhiteSpace(ext.Name))
				Name = ext.Name;

			if (Extension.BaseEntityNameExtended && !string.IsNullOrWhiteSpace(ext.BaseEntityName))
				BaseEntityName = ext.BaseEntityName;

			if (Extension.CustomBaseEntityNameExtended && !string.IsNullOrWhiteSpace(ext.CustomBaseEntityName))
				CustomBaseEntityName = ext.CustomBaseEntityName;

			if (Extension.IsSealedExtended)
				IsSealed = ext.IsSealed;

			if (Extension.RepositoryNameExtended && !string.IsNullOrWhiteSpace(ext.RepositoryName))
				RepositoryName = ext.RepositoryName;

			if (Extension.UniqueNameExtended && !string.IsNullOrWhiteSpace(ext.UniqueName))
				UniqueName = ext.UniqueName;

			if (Extension.PluralizedNameExtended && !string.IsNullOrWhiteSpace(ext.PluralizedName))
				PluralizedName = ext.PluralizedName;

			if (Extension.ActivityTokenExtended && !string.IsNullOrWhiteSpace(ext.ActivityToken))
				ActivityToken = ext.ActivityToken;

			if (Extension.DbSetNameExtended && !string.IsNullOrWhiteSpace(ext.DbSetName))
				DbSetName = ext.DbSetName;

			if (Extension.ModelNamesExtended) ModelNames = ext.ModelNames;
			if (Extension.ContextNamesExtended) ContextNames = ext.ContextNames;
			if (Extension.GenerateModelExtended) GenerateModel = ext.GenerateModel;
			if (Extension.ConvertEntityExtended) ConvertEntity = ext.ConvertEntity;
			if (Extension.DbContextTypeExtended) DbContextType = ext.DbContextType;
			if (Extension.IsAuditEntryExtended) IsAuditEntry = ext.IsAuditEntry;
			if (Extension.IsSelfAuditableEntityExtended) IsSelfAuditableEntity = ext.IsSelfAuditableEntity;
			if (Extension.IsAuditableEntityExtended) IsAuditableEntity = ext.IsAuditableEntity;
			if (Extension.IsSynchronizableEntityExtended) IsSynchronizableEntity = ext.IsSynchronizableEntity;
			if (Extension.IsCorrelableEntityExtended) IsCorrelableEntity = ext.IsCorrelableEntity;
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

		ModelContextNames = new Dictionary<string, string>();

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

			if (ModelContextNames.ContainsKey(modelName))
				throw new InvalidOperationException($"{ID}: Multiple {nameof(ModelNames)} - {modelName}");

			if (ModelContextNames.ContainsValue(contextName))
				throw new InvalidOperationException($"{ID}: Multiple {nameof(ContextNames)} - {contextName}");

			ModelContextNames.Add(modelName, contextName);
		}

		if (ModelContextNames.Count == 0)
			throw new InvalidOperationException($"{ID}: {nameof(ModelContextNames)} is empty");
	}

	public PropertyBase GetProperty(string id)
	{
		return Properties.FirstOrDefault(x => x.ID == id);
	}

	public NavigationBase GetNavigation(string id)
	{
		return Navigations.FirstOrDefault(x => x.ID == id);
	}

	public BackNavigationBase GetBackNavigation(string id)
	{
		return BackNavigations.FirstOrDefault(x => x.ID == id);
	}

	public override string ToString()
	{
		return $"{Schema}.{TableName}";
	}

	public static EntityBase CreateDefault(string schemaName, string tableName)
	{
		var entityBase = new EntityBase();

		entityBase.Init(
			new DBAbstractions.Metamodel.Entity
			{
				Schema = schemaName,
				TableName = tableName
			},
			null);

		return entityBase;
	}

	public void ResetName()
	{
		var name = TableName.ToCammelCase();
		Name = name;
		UniqueName = name;
	}

	public void ResetPropertyNames()
	{
		foreach (var property in Properties)
			property.ResetName();
	}

	public void ResetNavigationNames()
	{
		foreach (var navigation in Navigations)
			navigation.ResetName();
	}

	public void ResetBackNavigationNames()
	{
		foreach (var backNavigation in BackNavigations)
			backNavigation.ResetName();
	}
}
