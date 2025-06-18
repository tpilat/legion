using Legion.Database.Metamodel;
using Newtonsoft.Json;

namespace Legion.Generators.AppGen.Model.Config;

[Serializable]
public class CodeGeneratorSettingsExtension
{
	[JsonProperty]
	public bool BaseNamespaceExtended { get; set; }

	[JsonProperty]
	public bool NamespacePartForContextExtended { get; set; }

	[JsonProperty]
	public bool NamespacePartForEntityModelExtended { get; set; }

	[JsonProperty]
	public bool NamespacePartForQueryModelExtended { get; set; }

	[JsonProperty]
	public bool NamespacePartForQueryServicesExtended { get; set; }

	[JsonProperty]
	public bool NamespacePartForServicesExtended { get; set; }

	[JsonProperty]
	public bool EntityContextNameExtended { get; set; }

	[JsonProperty]
	public bool QueryContextNameExtended { get; set; }

	[JsonProperty]
	public bool SchemaNameForAuditEntryTableExtended { get; set; }

	[JsonProperty]
	public bool AuditEntryTableNameExtended { get; set; }

	[JsonProperty]
	public bool SchemaNameForErrorTableExtended { get; set; }

	[JsonProperty]
	public bool ErrorTableNameExtended { get; set; }

	[JsonProperty]
	public bool SchemaNameForTraceTableExtended { get; set; }

	[JsonProperty]
	public bool TraceTableNameExtended { get; set; }

	[JsonProperty]
	public bool SchemaNameForEntityTableExtended { get; set; }

	[JsonProperty]
	public bool EntityTableNameExtended { get; set; }

	[JsonProperty]
	public bool SchemaNameForOperationTableExtended { get; set; }

	[JsonProperty]
	public bool OperationTableNameExtended { get; set; }

	[JsonProperty]
	public bool SchemaNameForActivityTableExtended { get; set; }

	[JsonProperty]
	public bool ActivityTableNameExtended { get; set; }

	[JsonProperty]
	public bool SchemaNameForUserTableExtended { get; set; }

	[JsonProperty]
	public bool UserTableNameExtended { get; set; }

	[JsonProperty]
	public bool UserTableADsAMAccountNamePropertyExtended { get; set; }
}


[Serializable]
public class CodeGeneratorSettings
{
	[JsonProperty]
	public CodeGeneratorSettingsExtension Extension { get; set; }

	[JsonProperty]
	public string BaseNamespace { get; set; } = "Test";

	[JsonProperty]
	public string NamespacePartForContext { get; set; } = "PostgreSQL";

	public string Namespace_EntityModel(string modelName) => $"{BaseNamespace}.{modelName}";

	public string Namespace_QueryModel(string queryModelName) => $"{BaseNamespace}.{queryModelName}";

	public string Namespace_EntityContextModelBuilder(string modelName)
		=> $"{BaseNamespace}.{modelName}.{NamespacePartForContext}";

	public string Namespace_QueryContextModelBuilder(string modelName)
		=> $"{BaseNamespace}.{modelName}.{NamespacePartForContext}";

	public string Namespace_EntityContextAbstractionBuilder(string modelName)
		=> $"{BaseNamespace}.{modelName}.{NamespacePartForContext}";

	public string Namespace_QueryContextAbstractionBuilder(string modelName)
		=> $"{BaseNamespace}.{modelName}.{NamespacePartForContext}";

	[JsonProperty]
	public string SchemaNameForAuditEntryTable { get; set; } = "aud";

	[JsonProperty]
	public string AuditEntryTableName { get; set; } = "AuditEntry";

	[JsonIgnore]
	public string AuditEntryId => $"{SchemaNameForAuditEntryTable}.{AuditEntryTableName}";

	public static CodeGeneratorSettings CreateDefaultSettings(DatabaseProviderType providerType)
		=> new()
		{
			NamespacePartForContext = providerType == DatabaseProviderType.PostgreSQL
				? "PostgreSQL"
				: "SqlServer"
		};
}
