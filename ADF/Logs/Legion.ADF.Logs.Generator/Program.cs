using Legion.Generators;
//TODO RESX using Legion.ADF.Logs.Resources;
using System.Globalization;

namespace Legion.ADF.Logs.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Logs.Generator";
		//var targetProject = "Legion.ADF.Logs.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.Logs.Resources.Localizers).Assembly,
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
				RepoName = "Legion.ADF.Logs Repositories",
				ModelNamespace = "Legion.ADF.Logs",
				EFNamespace = "Legion.ADF.Logs.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs.PostgreSQL",
				ContextName = "Legion.ADF.Logs.PostgreSQL.ILogsDbContext",
				UnitOfWorkName = "LogsUnitOfWork",
				IRepositry = "Legion.ADF.Logs.ILogsRepository",
				RepositoryBase = "Legion.ADF.Logs.PostgreSQL.LogsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Logs.LogsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Logs.LogsBaseEntity),
					typeof(Legion.ADF.Logs.PostgreSQL.ILogsDbContext),
					typeof(Legion.ADF.Logs.ILogsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Logs QueryRepositories",
				ModelNamespace = "Legion.ADF.Logs",
				EFNamespace = "Legion.ADF.Logs.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs.PostgreSQL",
				ContextName = "Legion.ADF.Logs.PostgreSQL.ILogsQueryDbContext",
				UnitOfWorkName = "LogsQueryUnitOfWork",
				IRepositry = "Legion.ADF.Logs.ILogsQueryRepository",
				RepositoryBase = "Legion.ADF.Logs.PostgreSQL.LogsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Logs.LogsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Logs.LogsBaseQueryEntity),
					typeof(Legion.ADF.Logs.PostgreSQL.ILogsQueryDbContext),
					typeof(Legion.ADF.Logs.ILogsQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Logs Repositories",
				ModelNamespace = "Legion.ADF.Logs",
				EFNamespace = "Legion.ADF.Logs.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs.SqlServer",
				ContextName = "Legion.ADF.Logs.SqlServer.ILogsDbContext",
				UnitOfWorkName = "LogsUnitOfWork",
				IRepositry = "Legion.ADF.Logs.ILogsRepository",
				RepositoryBase = "Legion.ADF.Logs.SqlServer.LogsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Logs.LogsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Logs.LogsBaseEntity),
					typeof(Legion.ADF.Logs.SqlServer.ILogsDbContext),
					typeof(Legion.ADF.Logs.ILogsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Logs QueryRepositories",
				ModelNamespace = "Legion.ADF.Logs",
				EFNamespace = "Legion.ADF.Logs.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Logs.SqlServer",
				ContextName = "Legion.ADF.Logs.SqlServer.ILogsQueryDbContext",
				UnitOfWorkName = "LogsQueryUnitOfWork",
				IRepositry = "Legion.ADF.Logs.ILogsQueryRepository",
				RepositoryBase = "Legion.ADF.Logs.SqlServer.LogsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Logs.LogsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Logs.LogsBaseQueryEntity),
					typeof(Legion.ADF.Logs.SqlServer.ILogsQueryDbContext),
					typeof(Legion.ADF.Logs.ILogsQueryRepository)
				]
			});
	}
}

