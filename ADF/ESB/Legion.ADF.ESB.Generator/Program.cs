using Legion.Generators;
//TODO RESX using Legion.ADF.ESB.Resources;
using System.Globalization;

namespace Legion.ADF.ESB.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.ESB.Generator";
		//var targetProject = "Legion.ADF.ESB.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.ESB.Resources.Localizers).Assembly,
		//	defaultCulture,
		//	//TODO: Model.CodeList.SupportedLanguage.AsEnumerable().Select(x => new CultureInfo(x.ISO_639_1)).ToList(),
		//	new List<CultureInfo> { CultureInfo.GetCultureInfo("sk") },
		//	true);

		//Console.WriteLine("SUCCESS: Resources");

		var solutionDirectoryPath = args[0];

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ESB.Components Repositories",
				ModelNamespace = "Legion.ADF.ESB.Components",
				EFNamespace = "Legion.ADF.ESB.Components.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Components",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Components.PostgreSQL",
				ContextName = "Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext",
				UnitOfWorkName = "ComponentsUnitOfWork",
				IRepositry = "Legion.ADF.ESB.Components.IComponentsRepository",
				RepositoryBase = "Legion.ADF.ESB.Components.PostgreSQL.ComponentsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ESB.Components.ComponentsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ESB.Components.ComponentsBaseEntity),
					typeof(Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext),
					typeof(Legion.ADF.ESB.Components.IComponentsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ESB.Components QueryRepositories",
				ModelNamespace = "Legion.ADF.ESB.Components",
				EFNamespace = "Legion.ADF.ESB.Components.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Components",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Components.PostgreSQL",
				ContextName = "Legion.ADF.ESB.Components.PostgreSQL.IComponentsQueryDbContext",
				UnitOfWorkName = "ComponentsQueryUnitOfWork",
				IRepositry = "Legion.ADF.ESB.Components.IComponentsQueryRepository",
				RepositoryBase = "Legion.ADF.ESB.Components.PostgreSQL.ComponentsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ESB.Components.ComponentsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ESB.Components.ComponentsBaseQueryEntity),
					typeof(Legion.ADF.ESB.Components.PostgreSQL.IComponentsQueryDbContext),
					typeof(Legion.ADF.ESB.Components.IComponentsQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ESB.MBox Repositories",
				ModelNamespace = "Legion.ADF.ESB.MBox",
				EFNamespace = "Legion.ADF.ESB.MBox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.MBox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.MBox.PostgreSQL",
				ContextName = "Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext",
				UnitOfWorkName = "MBoxUnitOfWork",
				IRepositry = "Legion.ADF.ESB.MBox.IMBoxRepository",
				RepositoryBase = "Legion.ADF.ESB.MBox.PostgreSQL.MBoxRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ESB.MBox.MBoxBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ESB.MBox.MBoxBaseEntity),
					typeof(Legion.ADF.ESB.MBox.PostgreSQL.IMBoxDbContext),
					typeof(Legion.ADF.ESB.MBox.IMBoxRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ESB.MBox QueryRepositories",
				ModelNamespace = "Legion.ADF.ESB.MBox",
				EFNamespace = "Legion.ADF.ESB.MBox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.MBox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.MBox.PostgreSQL",
				ContextName = "Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext",
				UnitOfWorkName = "MBoxQueryUnitOfWork",
				IRepositry = "Legion.ADF.ESB.MBox.IMBoxQueryRepository",
				RepositoryBase = "Legion.ADF.ESB.MBox.PostgreSQL.MBoxQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ESB.MBox.MBoxBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ESB.MBox.MBoxBaseQueryEntity),
					typeof(Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext),
					typeof(Legion.ADF.ESB.MBox.IMBoxQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ESB.Orchestrations Repositories",
				ModelNamespace = "Legion.ADF.ESB.Orchestrations",
				EFNamespace = "Legion.ADF.ESB.Orchestrations.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Orchestrations",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Orchestrations.PostgreSQL",
				ContextName = "Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext",
				UnitOfWorkName = "OrchestrationsUnitOfWork",
				IRepositry = "Legion.ADF.ESB.Orchestrations.IOrchestrationsRepository",
				RepositoryBase = "Legion.ADF.ESB.Orchestrations.PostgreSQL.OrchestrationsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ESB.Orchestrations.OrchestrationsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ESB.Orchestrations.OrchestrationsBaseEntity),
					typeof(Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext),
					typeof(Legion.ADF.ESB.Orchestrations.IOrchestrationsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ESB.Orchestrations QueryRepositories",
				ModelNamespace = "Legion.ADF.ESB.Orchestrations",
				EFNamespace = "Legion.ADF.ESB.Orchestrations.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Orchestrations",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ESB.Orchestrations.PostgreSQL",
				ContextName = "Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext",
				UnitOfWorkName = "OrchestrationsQueryUnitOfWork",
				IRepositry = "Legion.ADF.ESB.Orchestrations.IOrchestrationsQueryRepository",
				RepositoryBase = "Legion.ADF.ESB.Orchestrations.PostgreSQL.OrchestrationsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ESB.Orchestrations.OrchestrationsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ESB.Orchestrations.OrchestrationsBaseQueryEntity),
					typeof(Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext),
					typeof(Legion.ADF.ESB.Orchestrations.IOrchestrationsQueryRepository)
				]
			});
	}
}

