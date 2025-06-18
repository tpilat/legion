using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Descriptors;

public class QueryContextDescriptor : TypeDescriptor<ModelBase>
{
	public string AbstractionTargetFolder { get; set; }
	public string AbstractionName { get; set; }
	public string AbstractionFileName { get; set; }
	public string AbstractionBaseNamespace { get; set; }
	public string AbstractionNamespace { get; set; }
	public List<string> AbstractionImports { get; set; }

	public QueryContextDescriptor(ModelBase data, GeneratorContext context)
		: base(data, context)
	{
		Model = Data;

		this.AddImports(new List<string>
		{
			"Microsoft.EntityFrameworkCore",
			"Microsoft.EntityFrameworkCore.Metadata"
		});

		AbstractionImports = new List<string>
		{
			"Microsoft.EntityFrameworkCore"
		};
	}

	public override void Initialize(string modelName, string contextName)
	{
		Name = contextName;
		FileName = $"{Name}.cs";

		AbstractionTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_QueryContextAbstractionBuilder(modelName));
		AbstractionName = $"I{contextName}";
		AbstractionFileName = $"{AbstractionName}.cs";
		AbstractionBaseNamespace = Settings.Namespace_QueryContextAbstractionBuilder(modelName);
		AbstractionNamespace = Settings.Namespace_QueryContextAbstractionBuilder(modelName);

		this.BuildImports(modelName, contextName);
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_QueryContextModelBuilder(modelName));

	public override string Namespace(string modelName, string contextName)
		=> Settings.Namespace_QueryContextModelBuilder(modelName);

	public override string BaseNamespace(string modelName, string contextName)
		=> modelName;

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);
		Context.QueryContextDescriptor.Initialize(modelName, contextName);

		if (Data.QueryEntities.All(e => Context.GetQueryModelDescriptor(e, modelName, contextName) == null || !e.GenerateQueryModel))
			return ModelResult;

		var result =
			GeneratorInvoker
				.Generate<QueryContextGenerator>(
					Context.QueryContextDescriptor.FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(QueryContextDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		ModelResult.MergeAllMessages(result);

		result =
			GeneratorInvoker
				.Generate<QueryContextModelBuilderGenerator>(
					Context.QueryContextDescriptor.FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(QueryContextDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
