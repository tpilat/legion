using Legion.Generators;
//TODO RESX using Legion.ADF.ServiceBus.Resources;
using System.Globalization;

namespace Legion.ADF.ServiceBus.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.ServiceBus.Generator";
		//var targetProject = "Legion.ADF.ServiceBus.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.ServiceBus.Resources.Localizers).Assembly,
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
				RepoName = "Legion.ADF.ServiceBus.Jobs Repositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Jobs",
				EFNamespace = "Legion.ADF.ServiceBus.Jobs.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs.PostgreSQL",
				ContextName = "Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsDbContext",
				UnitOfWorkName = "JobsUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Jobs.IJobsRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Jobs.PostgreSQL.JobsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Jobs.JobsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Jobs.JobsBaseEntity),
					typeof(Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsDbContext),
					typeof(Legion.ADF.ServiceBus.Jobs.IJobsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus.Jobs QueryRepositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Jobs",
				EFNamespace = "Legion.ADF.ServiceBus.Jobs.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs.PostgreSQL",
				ContextName = "Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsQueryDbContext",
				UnitOfWorkName = "JobsQueryUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Jobs.IJobsQueryRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Jobs.PostgreSQL.JobsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Jobs.JobsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Jobs.JobsBaseQueryEntity),
					typeof(Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsQueryDbContext),
					typeof(Legion.ADF.ServiceBus.Jobs.IJobsQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus.Jobs Repositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Jobs",
				EFNamespace = "Legion.ADF.ServiceBus.Jobs.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs.SqlServer",
				ContextName = "Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsDbContext",
				UnitOfWorkName = "JobsUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Jobs.IJobsRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Jobs.SqlServer.JobsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Jobs.JobsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Jobs.JobsBaseEntity),
					typeof(Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsDbContext),
					typeof(Legion.ADF.ServiceBus.Jobs.IJobsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus.Jobs QueryRepositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Jobs",
				EFNamespace = "Legion.ADF.ServiceBus.Jobs.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Jobs.SqlServer",
				ContextName = "Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsQueryDbContext",
				UnitOfWorkName = "JobsQueryUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Jobs.IJobsQueryRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Jobs.SqlServer.JobsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Jobs.JobsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Jobs.JobsBaseQueryEntity),
					typeof(Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsQueryDbContext),
					typeof(Legion.ADF.ServiceBus.Jobs.IJobsQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus.Orchestrations Repositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Orchestrations",
				EFNamespace = "Legion.ADF.ServiceBus.Orchestrations.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations.PostgreSQL",
				ContextName = "Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsDbContext",
				UnitOfWorkName = "OrchestrationsUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.OrchestrationsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseEntity),
					typeof(Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsDbContext),
					typeof(Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus.Orchestrations QueryRepositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Orchestrations",
				EFNamespace = "Legion.ADF.ServiceBus.Orchestrations.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations.PostgreSQL",
				ContextName = "Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext",
				UnitOfWorkName = "OrchestrationsQueryUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsQueryRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.OrchestrationsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseQueryEntity),
					typeof(Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext),
					typeof(Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus.Orchestrations Repositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Orchestrations",
				EFNamespace = "Legion.ADF.ServiceBus.Orchestrations.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations.SqlServer",
				ContextName = "Legion.ADF.ServiceBus.Orchestrations.SqlServer.IOrchestrationsDbContext",
				UnitOfWorkName = "OrchestrationsUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Orchestrations.SqlServer.OrchestrationsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseEntity),
					typeof(Legion.ADF.ServiceBus.Orchestrations.SqlServer.IOrchestrationsDbContext),
					typeof(Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus.Orchestrations QueryRepositories",
				ModelNamespace = "Legion.ADF.ServiceBus.Orchestrations",
				EFNamespace = "Legion.ADF.ServiceBus.Orchestrations.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.Orchestrations.SqlServer",
				ContextName = "Legion.ADF.ServiceBus.Orchestrations.SqlServer.IOrchestrationsQueryDbContext",
				UnitOfWorkName = "OrchestrationsQueryUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsQueryRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.Orchestrations.SqlServer.OrchestrationsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.Orchestrations.OrchestrationsBaseQueryEntity),
					typeof(Legion.ADF.ServiceBus.Orchestrations.SqlServer.IOrchestrationsQueryDbContext),
					typeof(Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsQueryRepository)
				]
			});
	}
}

