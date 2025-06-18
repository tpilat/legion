using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Descriptors;

public class EntityModelDescriptor : EntityBaseDescriptor
{
	private BaseEntityDescriptor _baseEntityDesc;
	public BaseEntityDescriptor BaseEntityDesc => _baseEntityDesc ?? (_baseEntityDesc = Context.BaseEntityDescriptors[Data.BaseEntityName]);
	public string GetBaseEntityName(string modelName, string contextName) => Data.CustomBaseEntityName ?? BaseEntityDesc.BaseName(modelName, contextName);

	public string EntityContextModelBuilderTargetFolder { get; set; }
	public string EntityContextModelBuilderName { get; set; }
	public string EntityContextModelBuilderFileName { get; set; }
	public string EntityContextModelBuilderBaseNamespace { get; set; }
	public string EntityContextModelBuilderNamespace { get; set; }
	public List<string> EntityContextModelBuilderImports { get; set; }
	public string EntityContextModelBuilderBaseName => $"{EntityContextModelBuilderBaseNamespace}.{EntityContextModelBuilderName}";
	public string EntityContextModelBuilderFullName => $"{EntityContextModelBuilderNamespace}.{EntityContextModelBuilderName}";


	public string EntityMapperTargetFolder { get; set; }
	public string EntityMapperFileName { get; set; }
	public List<string> EntityMapperImports { get; set; }


	public string ComparerName { get; set; }
	public string EntityEqualityComparerTargetFolder { get; set; }
	public string EntityEqualityComparerFileName { get; set; }
	public List<string> EntityEqualityComparerImports { get; set; }

	public EntityModelDescriptor(EntityBase entity, GeneratorContext context)
		: base(
			  entity ?? throw new ArgumentNullException(nameof(entity)),
			  context ?? throw new ArgumentNullException(nameof(context)))
	{
		Model = Data.Model;

		Name = Data.Name;
		FileName = $"{Name}.cs";

		this.AddImports(new List<string>
		{
			"Legion.Validation"
		});

		EntityContextModelBuilderImports = new List<string>
		{
			"Microsoft.EntityFrameworkCore",
			"Microsoft.EntityFrameworkCore.Metadata.Builders"
		};

		EntityMapperImports = new List<string>
		{
			"Legion",
			"Legion.Model.Mappers"
		};

		EntityEqualityComparerImports = new List<string>
		{
			"Legion",
			"Legion.Model.Comparers",
			"System.Diagnostics.CodeAnalysis"
		};

		this.AddImports(Data.Properties.SelectMany(p => p.Namespaces).Where(x => x != "System"));
	}

	public override void Initialize(string modelName, string contextName)
	{
		EntityContextModelBuilderTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityContextModelBuilder(modelName)/*, Data.Package.PathPart*/, "EFConfigurations");
		EntityContextModelBuilderName = $"{Data.Name}Configuration";
		EntityContextModelBuilderFileName = $"{EntityContextModelBuilderName}.cs";
		EntityContextModelBuilderBaseNamespace = $"{Settings.NamespacePartForContext}";
		EntityContextModelBuilderNamespace = $"{Settings.Namespace_EntityContextModelBuilder(modelName)}";
		//EntityContextModelBuilderBaseNamespace = $"{Settings.NamespacePartForContext}.{Data.Package.NamespacePart}";
		//EntityContextModelBuilderNamespace = $"{Settings.Namespace_EntityContextModelBuilder(modelName)}.{Data.Package.NamespacePart}";

		EntityMapperTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityModel(modelName), "Model"/*, Data.Package.PathPart*/, "Mappers");
		EntityMapperFileName = $"{Name}.Mapper.cs";

		ComparerName = $"{Name}EqualityComparer";
		EntityEqualityComparerTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityModel(modelName), "Model"/*, Data.Package.PathPart*/, "EqualityComparers");
		EntityEqualityComparerFileName = $"{Name}.EqualityComparer.cs";

		this.BuildImports(modelName, contextName);
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_EntityModel(modelName), "Model"/*, Data.Package.PathPart*/);

	public override string Namespace(string modelName, string contextName)
		//=> $"{Settings.Namespace_EntityModel(modelName)}.{Data.Package.NamespacePart}";
		=> $"{Settings.Namespace_EntityModel(modelName)}.Model";

	public override string BaseNamespace(string modelName, string contextName)
		//=> $"{modelName}.{Data.Package.NamespacePart}";
		=> $"{modelName}.Model";

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		var result =
			GeneratorInvoker
				.Generate<EntityModelGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EntityModelDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}

	public ModelResult GenerateMapper(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		var result =
			GeneratorInvoker
				.Generate<EntityMapperGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EntityModelDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}

	public ModelResult GenerateEqualityComparer(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		var result =
			GeneratorInvoker
				.Generate<EntityEqualityComparerGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(EntityModelDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
