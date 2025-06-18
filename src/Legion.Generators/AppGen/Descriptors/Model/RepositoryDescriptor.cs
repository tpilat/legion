using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Descriptors;

public class RepositoryDescriptor : TypeDescriptor<ModelBase>
{
	public string InterfaceTargetFolder { get; set; }
	public string InterfaceNamespace { get; set; }
	public string InterfaceName { get; set; }
	public string InterfaceFileName { get; set; }
	public List<string> InterfaceImports { get; set; }

	public string BaseRepoTargetFolder { get; set; }
	public string BaseRepoNamespace { get; set; }
	public string BaseRepoName { get; set; }
	public string BaseRepoFileName { get; set; }
	public List<string> BaseRepoImports { get; set; }

	public string DbContextFullName { get; set; }

	public RepositoryDescriptor(string repositoryName, ModelBase data, GeneratorContext context)
		: base(data, context)
	{
		Model = Data;

		Name = repositoryName;
		FileName = $"{Name}.cs";

		this.AddImports(new List<string>
		{
		});
	}

	public override void Initialize(string modelName, string contextName)
	{
		InterfaceTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityModel(modelName));
		InterfaceNamespace = $"{Settings.BaseNamespace}.{modelName}";
		InterfaceName = $"I{Name}";
		InterfaceFileName = $"{InterfaceName}.cs";
		InterfaceImports = new List<string>();

		BaseRepoTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityContextModelBuilder(modelName));
		BaseRepoNamespace = Settings.Namespace_EntityContextModelBuilder(modelName);
		BaseRepoName = $"{Name}Base";
		BaseRepoFileName = $"{BaseRepoName}.cs";
		BaseRepoImports = new List<string>
		{
			"Legion",
			"Legion.EntityFrameworkCore",
			"Legion.Extensions",
			"Legion.Model.Audit",
			"Legion.Model.Repositories",
			CodeGeneratorConfig.Instance.SelectedDatabaseConnection.Provider == Database.Metamodel.DatabaseProviderType.PostgreSQL ? "Npgsql" : "Microsoft.Data.SqlClient"
		};

		DbContextFullName = $"{Settings.Namespace_EntityContextAbstractionBuilder(modelName)}.I{contextName}";

		this.BuildImports(modelName, contextName);
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityContextModelBuilder(modelName));

	public override string Namespace(string modelName, string contextName)
		=> Settings.Namespace_EntityContextModelBuilder(modelName);

	public override string BaseNamespace(string modelName, string contextName)
		=> Settings.Namespace_EntityContextModelBuilder(modelName);

	public void GetDbContext(string modelName)
	{
		Model.ModelContextDict.TryGetValue(modelName, out var context);
	}

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		var result =
			GeneratorInvoker
				.Generate<RepositoryGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(RepositoryDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
