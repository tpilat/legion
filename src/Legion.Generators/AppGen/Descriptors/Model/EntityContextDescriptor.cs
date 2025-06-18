using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Descriptors;

public class EntityContextDescriptor : TypeDescriptor<ModelBase>
{
	public string AbstractionTargetFolder { get; set; }
	public string AbstractionName { get; set; }
	public string AbstractionFileName { get; set; }
	public string AbstractionBaseNamespace { get; set; }
	public string AbstractionNamespace { get; set; }
	public List<string> AbstractionImports { get; set; }

	public EntityContextDescriptor(ModelBase data, GeneratorContext context)
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

		AbstractionTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityContextAbstractionBuilder(modelName));
		AbstractionName = $"I{contextName}";
		AbstractionFileName = $"{AbstractionName}.cs";
		AbstractionBaseNamespace = Settings.Namespace_EntityContextAbstractionBuilder(modelName);
		AbstractionNamespace = Settings.Namespace_EntityContextAbstractionBuilder(modelName);

		this.BuildImports(modelName, contextName);
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityContextModelBuilder(modelName));

	public override string Namespace(string modelName, string contextName)
		=> Settings.Namespace_EntityContextModelBuilder(modelName);

	public override string BaseNamespace(string modelName, string contextName)
		=> modelName;

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);
		Context.EntityContextDescriptor.Initialize(modelName, contextName);

		if (Data.Entities.All(e => Context.GetEntityModelDescriptor(e, true, modelName, contextName) == null || !e.GenerateModel))
			return ModelResult;

		var result =
			GeneratorInvoker
				.Generate<EntityContextGenerator>(
					Context.EntityContextDescriptor.FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EntityContextDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		ModelResult.MergeAllMessages(result);

		result =
			GeneratorInvoker
				.Generate<EntityContextModelBuilderGenerator>(
					Context.EntityContextDescriptor.FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EntityContextDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
