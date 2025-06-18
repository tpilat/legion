using Legion.Generators;
//TODO RESX using Legion.ADF.Config.Resources;
using System.Globalization;

namespace Legion.ADF.Config.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Config.Generator";
		//var targetProject = "Legion.ADF.Config.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.Config.Resources.Localizers).Assembly,
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
				RepoName = "Legion.ADF.Config Repositories",
				ModelNamespace = "Legion.ADF.Config",
				EFNamespace = "Legion.ADF.Config.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config.PostgreSQL",
				ContextName = "Legion.ADF.Config.PostgreSQL.IConfigDbContext",
				UnitOfWorkName = "ConfigUnitOfWork",
				IRepositry = "Legion.ADF.Config.IConfigRepository",
				RepositoryBase = "Legion.ADF.Config.PostgreSQL.ConfigRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Config.ConfigBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Config.ConfigBaseEntity),
					typeof(Legion.ADF.Config.PostgreSQL.IConfigDbContext),
					typeof(Legion.ADF.Config.IConfigRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Config QueryRepositories",
				ModelNamespace = "Legion.ADF.Config",
				EFNamespace = "Legion.ADF.Config.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config.PostgreSQL",
				ContextName = "Legion.ADF.Config.PostgreSQL.IConfigQueryDbContext",
				UnitOfWorkName = "ConfigQueryUnitOfWork",
				IRepositry = "Legion.ADF.Config.IConfigQueryRepository",
				RepositoryBase = "Legion.ADF.Config.PostgreSQL.ConfigQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Config.ConfigBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Config.ConfigBaseQueryEntity),
					typeof(Legion.ADF.Config.PostgreSQL.IConfigQueryDbContext),
					typeof(Legion.ADF.Config.IConfigQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Config Repositories",
				ModelNamespace = "Legion.ADF.Config",
				EFNamespace = "Legion.ADF.Config.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config.SqlServer",
				ContextName = "Legion.ADF.Config.SqlServer.IConfigDbContext",
				UnitOfWorkName = "ConfigUnitOfWork",
				IRepositry = "Legion.ADF.Config.IConfigRepository",
				RepositoryBase = "Legion.ADF.Config.SqlServer.ConfigRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Config.ConfigBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Config.ConfigBaseEntity),
					typeof(Legion.ADF.Config.SqlServer.IConfigDbContext),
					typeof(Legion.ADF.Config.IConfigRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Config QueryRepositories",
				ModelNamespace = "Legion.ADF.Config",
				EFNamespace = "Legion.ADF.Config.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Config.SqlServer",
				ContextName = "Legion.ADF.Config.SqlServer.IConfigQueryDbContext",
				UnitOfWorkName = "ConfigQueryUnitOfWork",
				IRepositry = "Legion.ADF.Config.IConfigQueryRepository",
				RepositoryBase = "Legion.ADF.Config.SqlServer.ConfigQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Config.ConfigBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Config.ConfigBaseQueryEntity),
					typeof(Legion.ADF.Config.SqlServer.IConfigQueryDbContext),
					typeof(Legion.ADF.Config.IConfigQueryRepository)
				]
			});
	}
}

