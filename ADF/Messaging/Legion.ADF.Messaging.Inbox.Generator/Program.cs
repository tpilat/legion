using Legion.Generators;
//TODO RESX using Legion.ADF.Messaging.Resources;
using System.Globalization;

namespace Legion.ADF.Messaging.Inbox.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Messaging.Inbox.Generator";
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
				RepoName = "Legion.ADF.Messaging.Inbox Repositories",
				ModelNamespace = "Legion.ADF.Messaging.Inbox",
				EFNamespace = "Legion.ADF.Messaging.Inbox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxDbContext",
				UnitOfWorkName = "InboxUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Inbox.IInboxRepository",
				RepositoryBase = "Legion.ADF.Messaging.Inbox.PostgreSQL.InboxRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Inbox.InboxBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Inbox.InboxBaseEntity),
					typeof(Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxDbContext),
					typeof(Legion.ADF.Messaging.Inbox.IInboxRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.Inbox QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.Inbox",
				EFNamespace = "Legion.ADF.Messaging.Inbox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext",
				UnitOfWorkName = "InboxQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Inbox.IInboxQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.Inbox.PostgreSQL.InboxQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Inbox.InboxBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Inbox.InboxBaseQueryEntity),
					typeof(Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext),
					typeof(Legion.ADF.Messaging.Inbox.IInboxQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.Inbox Repositories",
				ModelNamespace = "Legion.ADF.Messaging.Inbox",
				EFNamespace = "Legion.ADF.Messaging.Inbox.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox.SqlServer",
				ContextName = "Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext",
				UnitOfWorkName = "InboxUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Inbox.IInboxRepository",
				RepositoryBase = "Legion.ADF.Messaging.Inbox.SqlServer.InboxRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Inbox.InboxBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Inbox.InboxBaseEntity),
					typeof(Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext),
					typeof(Legion.ADF.Messaging.Inbox.IInboxRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.Inbox QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.Inbox",
				EFNamespace = "Legion.ADF.Messaging.Inbox.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Inbox.SqlServer",
				ContextName = "Legion.ADF.Messaging.Inbox.SqlServer.IInboxQueryDbContext",
				UnitOfWorkName = "InboxQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Inbox.IInboxQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.Inbox.SqlServer.InboxQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Inbox.InboxBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Inbox.InboxBaseQueryEntity),
					typeof(Legion.ADF.Messaging.Inbox.SqlServer.IInboxQueryDbContext),
					typeof(Legion.ADF.Messaging.Inbox.IInboxQueryRepository)
				]
			});
	}
}

