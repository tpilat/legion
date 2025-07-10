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
				RepoName = "Legion.ADF.ServiceBus Repositories",
				ModelNamespace = "Legion.ADF.ServiceBus",
				EFNamespace = "Legion.ADF.ServiceBus.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.PostgreSQL",
				ContextName = "Legion.ADF.ServiceBus.PostgreSQL.IServiceBusDbContext",
				UnitOfWorkName = "ServiceBusUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.IServiceBusRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.PostgreSQL.ServiceBusRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.ServiceBusBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.ServiceBusBaseEntity),
					typeof(Legion.ADF.ServiceBus.PostgreSQL.IServiceBusDbContext),
					typeof(Legion.ADF.ServiceBus.IServiceBusRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus QueryRepositories",
				ModelNamespace = "Legion.ADF.ServiceBus",
				EFNamespace = "Legion.ADF.ServiceBus.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.PostgreSQL",
				ContextName = "Legion.ADF.ServiceBus.PostgreSQL.IServiceBusQueryDbContext",
				UnitOfWorkName = "ServiceBusQueryUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.IServiceBusQueryRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.PostgreSQL.ServiceBusQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.ServiceBusBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.ServiceBusBaseQueryEntity),
					typeof(Legion.ADF.ServiceBus.PostgreSQL.IServiceBusQueryDbContext),
					typeof(Legion.ADF.ServiceBus.IServiceBusQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus Repositories",
				ModelNamespace = "Legion.ADF.ServiceBus",
				EFNamespace = "Legion.ADF.ServiceBus.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.SqlServer",
				ContextName = "Legion.ADF.ServiceBus.SqlServer.IServiceBusDbContext",
				UnitOfWorkName = "ServiceBusUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.IServiceBusRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.SqlServer.ServiceBusRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.ServiceBusBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.ServiceBusBaseEntity),
					typeof(Legion.ADF.ServiceBus.SqlServer.IServiceBusDbContext),
					typeof(Legion.ADF.ServiceBus.IServiceBusRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.ServiceBus QueryRepositories",
				ModelNamespace = "Legion.ADF.ServiceBus",
				EFNamespace = "Legion.ADF.ServiceBus.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.ServiceBus.SqlServer",
				ContextName = "Legion.ADF.ServiceBus.SqlServer.IServiceBusQueryDbContext",
				UnitOfWorkName = "ServiceBusQueryUnitOfWork",
				IRepositry = "Legion.ADF.ServiceBus.IServiceBusQueryRepository",
				RepositoryBase = "Legion.ADF.ServiceBus.SqlServer.ServiceBusQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.ServiceBus.ServiceBusBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.ServiceBus.ServiceBusBaseQueryEntity),
					typeof(Legion.ADF.ServiceBus.SqlServer.IServiceBusQueryDbContext),
					typeof(Legion.ADF.ServiceBus.IServiceBusQueryRepository)
				]
			});
	}
}

