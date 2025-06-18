using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Descriptors;

public class BaseEntityDescriptor : TypeDescriptor<ModelBase>
{
	public BaseEntityDescriptor(string baseEntityName, ModelBase data, GeneratorContext context)
		: base(data, context)
	{
		Model = Data;

		Name = baseEntityName;
		FileName = $"{Name}.cs";

		this.AddImports(new List<string>
		{
		});
	}

	public override void Initialize(string modelName, string contextName)
	{
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityModel(modelName));

	public override string Namespace(string modelName, string contextName)
		=> $"{Settings.BaseNamespace}.{modelName}";

	public override string BaseNamespace(string modelName, string contextName)
		=> modelName;

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		var result =
			GeneratorInvoker
				.Generate<BaseEntityGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(BaseEntityDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
