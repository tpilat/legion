using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.AppGenGenerators.Model;
using Legion.Generators.AppGen.Descriptors.MetaDescriptors;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen.Descriptors;

public class QueryModelDescriptor : QueryEntityBaseDescriptor
{
	private BaseQueryEntityDescriptor _baseQueryEntityDesc;
	public BaseQueryEntityDescriptor BaseQueryEntityDesc => _baseQueryEntityDesc ?? (_baseQueryEntityDesc = Context.BaseQueryEntityDescriptors[Data.BaseQueryEntityName]);
	public string GetBaseQueryEntityName(string modelName, string contextName) => Data.CustomBaseQueryEntityName ?? BaseQueryEntityDesc.BaseName(modelName, contextName);

	public string QueryContextModelBuilderTargetFolder { get; set; }
	public string QueryContextModelBuilderName { get; set; }
	public string QueryContextModelBuilderFileName { get; set; }
	public string QueryContextModelBuilderBaseNamespace { get; set; }
	public string QueryContextModelBuilderNamespace { get; set; }
	public List<string> QueryContextModelBuilderImports { get; set; }
	public string QueryContextModelBuilderBaseName => $"{QueryContextModelBuilderBaseNamespace}.{QueryContextModelBuilderName}";
	public string QueryContextModelBuilderFullName => $"{QueryContextModelBuilderNamespace}.{QueryContextModelBuilderName}";

	public QueryModelDescriptor(QueryEntityBase queryEntity, GeneratorContext context)
		: base(
			  queryEntity ?? throw new ArgumentNullException(nameof(queryEntity)),
			  context ?? throw new ArgumentNullException(nameof(context)))
	{
		Model = Data.Model;

		Name = Data.Name;
		FileName = $"{Name}.cs";

		this.AddImports(new List<string>
		{
		});

		QueryContextModelBuilderImports = new List<string>
		{
			"Microsoft.EntityFrameworkCore",
			"Microsoft.EntityFrameworkCore.Metadata.Builders"
		};

		this
			.AddImports(Data.Properties.SelectMany(p => p.Namespaces).Where(x => x != "System"));
	}

	public override void Initialize(string modelName, string contextName)
	{
		QueryContextModelBuilderTargetFolder = Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_QueryContextModelBuilder(modelName)/*, Data.Package.PathPart*/, "EFConfigurations");
		QueryContextModelBuilderName = $"{Data.Name}Configuration";
		QueryContextModelBuilderFileName = $"{QueryContextModelBuilderName}.cs";
		QueryContextModelBuilderBaseNamespace = $"{Settings.NamespacePartForContext}";
		QueryContextModelBuilderNamespace = $"{Settings.Namespace_QueryContextModelBuilder(modelName)}";
		//QueryContextModelBuilderBaseNamespace = $"{Settings.NamespacePartForContext}.{Data.Package.NamespacePart}";
		//QueryContextModelBuilderNamespace = $"{Settings.Namespace_QueryContextModelBuilder(modelName)}.{Data.Package.NamespacePart}";

		this.BuildImports(modelName, contextName);
	}

	public override string TargetFolder(string modelName, string contextName)
		=> Path.Combine(CodeGeneratorConfig.Instance.WorkspacePath, Settings.Namespace_QueryModel(modelName), "Model"/*, Data.Package.PathPart*/);

	public override string Namespace(string modelName, string contextName)
		//=> $"{Settings.Namespace_QueryModel(modelName)}.{Data.Package.NamespacePart}";
		=> $"{Settings.Namespace_QueryModel(modelName)}.Model";

	public override string BaseNamespace(string modelName, string contextName)
		//=> $"{modelName}.{Data.Package.NamespacePart}";
		=> $"{modelName}.Model";

	public override ModelResult Generate(string modelName, string contextName)
	{
		Initialize(modelName, contextName);

		var result =
			GeneratorInvoker
				.Generate<QueryModelGenerator>(
					FilePath(modelName, contextName),
					new Dictionary<string, object> { { nameof(QueryModelDescriptor), this }, { nameof(modelName), modelName }, { nameof(contextName), contextName } });

		return ModelResult.MergeAllMessages(result);
	}
}
