using Legion.Generators;
//TODO RESX using Legion.ADF.Messaging.Resources;
using System.Globalization;

namespace Legion.ADF.Messaging.DomainEvents.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Messaging.DomainEvents.Generator";
		//var targetProject = "Legion.ADF.Messaging.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.Messaging.Resources.Localizers).Assembly,
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
				RepoName = "Legion.ADF.Messaging.DomainEvents Repositories",
				ModelNamespace = "Legion.ADF.Messaging.DomainEvents",
				EFNamespace = "Legion.ADF.Messaging.DomainEvents.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsDbContext",
				UnitOfWorkName = "DomainEventsUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository",
				RepositoryBase = "Legion.ADF.Messaging.DomainEvents.PostgreSQL.DomainEventsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.DomainEvents.DomainEventsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.DomainEvents.DomainEventsBaseEntity),
					typeof(Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsDbContext),
					typeof(Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.DomainEvents QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.DomainEvents",
				EFNamespace = "Legion.ADF.Messaging.DomainEvents.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsQueryDbContext",
				UnitOfWorkName = "DomainEventsQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.DomainEvents.IDomainEventsQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.DomainEvents.PostgreSQL.DomainEventsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.DomainEvents.DomainEventsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.DomainEvents.DomainEventsBaseQueryEntity),
					typeof(Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsQueryDbContext),
					typeof(Legion.ADF.Messaging.DomainEvents.IDomainEventsQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.DomainEvents Repositories",
				ModelNamespace = "Legion.ADF.Messaging.DomainEvents",
				EFNamespace = "Legion.ADF.Messaging.DomainEvents.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents.SqlServer",
				ContextName = "Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext",
				UnitOfWorkName = "DomainEventsUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository",
				RepositoryBase = "Legion.ADF.Messaging.DomainEvents.SqlServer.DomainEventsRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.DomainEvents.DomainEventsBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.DomainEvents.DomainEventsBaseEntity),
					typeof(Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext),
					typeof(Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.DomainEvents QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.DomainEvents",
				EFNamespace = "Legion.ADF.Messaging.DomainEvents.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.DomainEvents.SqlServer",
				ContextName = "Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsQueryDbContext",
				UnitOfWorkName = "DomainEventsQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.DomainEvents.IDomainEventsQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.DomainEvents.SqlServer.DomainEventsQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.DomainEvents.DomainEventsBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.DomainEvents.DomainEventsBaseQueryEntity),
					typeof(Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsQueryDbContext),
					typeof(Legion.ADF.Messaging.DomainEvents.IDomainEventsQueryRepository)
				]
			});
	}
}

