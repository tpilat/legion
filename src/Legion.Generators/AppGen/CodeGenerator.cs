using Legion.Extensions;
using Legion.Generators.AppGen.AppGenGenerators;
using Legion.Generators.AppGen.Model;
using Legion.Generators.AppGen.Model.Config;

namespace Legion.Generators.AppGen;


public class CodeGenerator
{
	public ModelResult ModelResult { get; }
	public GeneratorContext Context { get; }

	public CodeGenerator(ModelBase model)
	{
		if (model == null)
			throw new ArgumentNullException(nameof(model));

		ModelResult = new ModelResult();
		Context = new GeneratorContext(model);
	}

	public bool Generate()
	{
		Context.BuildDescriptors(ModelResult);
		if (ModelResult.HasError)
			return false;

		if (CodeGeneratorConfig.Instance.CleanWorkspace)
		{
			try
			{
				if (Directory.Exists(CodeGeneratorConfig.Instance.WorkspacePath))
				{
					Directory.Delete(CodeGeneratorConfig.Instance.WorkspacePath, true);
				}
				Directory.CreateDirectory(CodeGeneratorConfig.Instance.WorkspacePath);
			}
			catch (Exception ex)
			{
				ModelResult.AddError(nameof(CodeGeneratorConfig.Instance.CleanWorkspace),
					$"While Cleaning workspace: {CodeGeneratorConfig.Instance.WorkspacePath}{Environment.NewLine}{ex.ToStringTrace()}");
				return false;
			}
		}

		#region Model

		foreach (var kvp in Context.Model.ModelContextDict)
		{
			var modelName = kvp.Key;
			var contextName = kvp.Value;

			var baseEntityNames = new HashSet<string>();
			var repositoryNames = new HashSet<string>();

			foreach (var modelEntity in Context.GetAllEntityModelDescriptor(modelName, contextName))
			{
				ModelResult.MergeHasError(modelEntity.Generate(modelName, contextName));
				ModelResult.MergeHasError(modelEntity.GenerateMapper(modelName, contextName));
				ModelResult.MergeHasError(modelEntity.GenerateEqualityComparer(modelName, contextName));

				if (string.IsNullOrWhiteSpace(modelEntity.Data.CustomBaseEntityName))
					baseEntityNames.Add(modelEntity.Data.BaseEntityName);

				repositoryNames.Add(modelEntity.Data.RepositoryName);
			}

			foreach (var modelEntityEnum in Context.GetAllEntityModelEnumDescriptor(modelName, contextName))
			{
				ModelResult.MergeHasError(modelEntityEnum.Generate(modelName, contextName));

				if (string.IsNullOrWhiteSpace(modelEntityEnum.Data.CustomBaseEntityName))
					baseEntityNames.Add(modelEntityEnum.Data.BaseEntityName);

				repositoryNames.Add(modelEntityEnum.Data.RepositoryName);
			}

			foreach (var modelEntityEnumeration in Context.GetAllEntityModelEnumerationDescriptor(modelName, contextName))
			{
				ModelResult.MergeHasError(modelEntityEnumeration.Generate(modelName, contextName));

				if (string.IsNullOrWhiteSpace(modelEntityEnumeration.Data.CustomBaseEntityName))
					baseEntityNames.Add(modelEntityEnumeration.Data.BaseEntityName);

				repositoryNames.Add(modelEntityEnumeration.Data.RepositoryName);
			}

			foreach (var baseEntityName in baseEntityNames)
				ModelResult.MergeHasError(Context.BaseEntityDescriptors[baseEntityName].Generate(modelName, contextName));

			foreach (var repositoryName in repositoryNames)
			{
				ModelResult.MergeHasError(Context.RepositoryDescriptors[repositoryName].Generate(modelName, contextName));
				ModelResult.MergeHasError(Context.TableInfoDescriptors[repositoryName].Generate(modelName, contextName));
			}

			ModelResult.MergeHasError(Context.EntityContextDescriptor.Generate(modelName, contextName));
		}

		foreach (var kvp in Context.Model.QueryModelContextDict)
		{
			var modelName = kvp.Key;
			var contextName = kvp.Value;

			var baseQueryEntityNames = new HashSet<string>();
			var queryRepositoryNames = new HashSet<string>();

			foreach (var queryModelEntity in Context.GetAllQueryModelDescriptor(modelName, contextName))
			{
				ModelResult.MergeHasError(queryModelEntity.Generate(modelName, contextName));
				baseQueryEntityNames.Add(queryModelEntity.Data.BaseQueryEntityName);
				queryRepositoryNames.Add(queryModelEntity.Data.QueryRepositoryName);
			}

			foreach (var baseQueryEntityName in baseQueryEntityNames)
				ModelResult.MergeHasError(Context.BaseQueryEntityDescriptors[baseQueryEntityName].Generate(modelName, contextName));

			foreach (var queryRepositoryName in queryRepositoryNames)
			{
				ModelResult.MergeHasError(Context.QueryRepositoryDescriptors[queryRepositoryName].Generate(modelName, contextName));
				ModelResult.MergeHasError(Context.QueryTableInfoDescriptors[queryRepositoryName].Generate(modelName, contextName));
			}

			ModelResult.MergeHasError(Context.QueryContextDescriptor.Generate(modelName, contextName));
		}

		#endregion Model

		if (!ModelResult.HasAnyMessage)
			ModelResult.AddMessage("Generator", "Generation has been completed.");

		return true;
	}
}
