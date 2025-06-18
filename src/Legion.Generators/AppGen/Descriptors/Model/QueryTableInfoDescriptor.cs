using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Descriptors;

public class QueryTableInfoDescriptor : TypeDescriptor<ModelBase>
{
	public string InterfaceTargetFolder { get; set; }
	public string InterfaceNamespace { get; set; }
	public string InterfaceName { get; set; }
	public string InterfaceFileName { get; set; }
	public List<string> InterfaceImports { get; set; }

	public string TableInfoTargetFolder { get; set; }
	public string TableInfoNamespace { get; set; }

	public string DbContextFullName { get; set; }

	public QueryTableInfoDescriptor(ModelBase data, GeneratorContext context)
		: base(data, context)
	{
		Model = Data;

		Name = "QueryTableInfoProvider";
		FileName = $"{Name}.cs";

		this.AddImports(new List<string>
		{
			"Legion.Extensions"
		});
	}

	public override void Initialize(string modelName, string contextName)
	{
		InterfaceTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_QueryModel(modelName));
		InterfaceNamespace = $"{Settings.BaseNamespace}.{modelName}";
		InterfaceName = $"I{Name}";
		InterfaceFileName = $"{InterfaceName}.cs";
		InterfaceImports = new List<string>();

		TableInfoTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_QueryContextModelBuilder(modelName));
		TableInfoNamespace = Settings.Namespace_QueryContextModelBuilder(modelName);

		DbContextFullName = $"{Settings.Namespace_EntityContextAbstractionBuilder(modelName)}.I{contextName}";

		this.BuildImports(modelName, contextName);
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_QueryContextModelBuilder(modelName));

	public override string Namespace(string modelName, string contextName)
		=> Settings.Namespace_QueryContextModelBuilder(modelName);

	public override string BaseNamespace(string modelName, string contextName)
		=> Settings.Namespace_QueryContextModelBuilder(modelName);

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		var result =
			GeneratorInvoker
				.Generate<QueryTableInfoGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(QueryTableInfoDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
