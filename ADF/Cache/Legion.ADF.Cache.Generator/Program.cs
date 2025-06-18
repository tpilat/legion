using Legion.Generators;
//TODO RESX using Legion.ADF.Cache.Resources;
using System.Globalization;

namespace Legion.ADF.Cache.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Cache.Generator";
		//var targetProject = "Legion.ADF.Cache.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.Cache.Resources.Localizers).Assembly,
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
				RepoName = "Legion.ADF.Cache Repositories",
				ModelNamespace = "Legion.ADF.Cache",
				EFNamespace = "Legion.ADF.Cache.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache.PostgreSQL",
				ContextName = "Legion.ADF.Cache.PostgreSQL.ICacheDbContext",
				UnitOfWorkName = "CacheUnitOfWork",
				IRepositry = "Legion.ADF.Cache.ICacheRepository",
				RepositoryBase = "Legion.ADF.Cache.PostgreSQL.CacheRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Cache.CacheBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Cache.CacheBaseEntity),
					typeof(Legion.ADF.Cache.PostgreSQL.ICacheDbContext),
					typeof(Legion.ADF.Cache.ICacheRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Cache QueryRepositories",
				ModelNamespace = "Legion.ADF.Cache",
				EFNamespace = "Legion.ADF.Cache.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache.PostgreSQL",
				ContextName = "Legion.ADF.Cache.PostgreSQL.ICacheQueryDbContext",
				UnitOfWorkName = "CacheQueryUnitOfWork",
				IRepositry = "Legion.ADF.Cache.ICacheQueryRepository",
				RepositoryBase = "Legion.ADF.Cache.PostgreSQL.CacheQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Cache.CacheBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Cache.CacheBaseQueryEntity),
					typeof(Legion.ADF.Cache.PostgreSQL.ICacheQueryDbContext),
					typeof(Legion.ADF.Cache.ICacheQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Cache Repositories",
				ModelNamespace = "Legion.ADF.Cache",
				EFNamespace = "Legion.ADF.Cache.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache.SqlServer",
				ContextName = "Legion.ADF.Cache.SqlServer.ICacheDbContext",
				UnitOfWorkName = "CacheUnitOfWork",
				IRepositry = "Legion.ADF.Cache.ICacheRepository",
				RepositoryBase = "Legion.ADF.Cache.SqlServer.CacheRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Cache.CacheBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Cache.CacheBaseEntity),
					typeof(Legion.ADF.Cache.SqlServer.ICacheDbContext),
					typeof(Legion.ADF.Cache.ICacheRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Cache QueryRepositories",
				ModelNamespace = "Legion.ADF.Cache",
				EFNamespace = "Legion.ADF.Cache.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Cache.SqlServer",
				ContextName = "Legion.ADF.Cache.SqlServer.ICacheQueryDbContext",
				UnitOfWorkName = "CacheQueryUnitOfWork",
				IRepositry = "Legion.ADF.Cache.ICacheQueryRepository",
				RepositoryBase = "Legion.ADF.Cache.SqlServer.CacheQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Cache.CacheBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Cache.CacheBaseQueryEntity),
					typeof(Legion.ADF.Cache.SqlServer.ICacheQueryDbContext),
					typeof(Legion.ADF.Cache.ICacheQueryRepository)
				]
			});
	}
}

