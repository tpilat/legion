using Legion.Generators;
//TODO RESX using Legion.ADF.Messaging.Resources;
using System.Globalization;

namespace Legion.ADF.Messaging.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Messaging.Generator";
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

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.MessageBox Repositories",
				ModelNamespace = "Legion.ADF.Messaging.MessageBox",
				EFNamespace = "Legion.ADF.Messaging.MessageBox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext",
				UnitOfWorkName = "MessageBoxUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.MessageBox.IMessageBoxRepository",
				RepositoryBase = "Legion.ADF.Messaging.MessageBox.PostgreSQL.MessageBoxRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.MessageBox.MessageBoxBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.MessageBox.MessageBoxBaseEntity),
					typeof(Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext),
					typeof(Legion.ADF.Messaging.MessageBox.IMessageBoxRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.MessageBox QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.MessageBox",
				EFNamespace = "Legion.ADF.Messaging.MessageBox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext",
				UnitOfWorkName = "MessageBoxQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.MessageBox.PostgreSQL.MessageBoxQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.MessageBox.MessageBoxBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.MessageBox.MessageBoxBaseQueryEntity),
					typeof(Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext),
					typeof(Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.MessageBox Repositories",
				ModelNamespace = "Legion.ADF.Messaging.MessageBox",
				EFNamespace = "Legion.ADF.Messaging.MessageBox.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox.SqlServer",
				ContextName = "Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext",
				UnitOfWorkName = "MessageBoxUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.MessageBox.IMessageBoxRepository",
				RepositoryBase = "Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.MessageBox.MessageBoxBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.MessageBox.MessageBoxBaseEntity),
					typeof(Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext),
					typeof(Legion.ADF.Messaging.MessageBox.IMessageBoxRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.MessageBox QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.MessageBox",
				EFNamespace = "Legion.ADF.Messaging.MessageBox.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.MessageBox.SqlServer",
				ContextName = "Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext",
				UnitOfWorkName = "MessageBoxQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.MessageBox.MessageBoxBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.MessageBox.MessageBoxBaseQueryEntity),
					typeof(Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext),
					typeof(Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.Outbox Repositories",
				ModelNamespace = "Legion.ADF.Messaging.Outbox",
				EFNamespace = "Legion.ADF.Messaging.Outbox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxDbContext",
				UnitOfWorkName = "OutboxUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Outbox.IOutboxRepository",
				RepositoryBase = "Legion.ADF.Messaging.Outbox.PostgreSQL.OutboxRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Outbox.OutboxBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Outbox.OutboxBaseEntity),
					typeof(Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxDbContext),
					typeof(Legion.ADF.Messaging.Outbox.IOutboxRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.Outbox QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.Outbox",
				EFNamespace = "Legion.ADF.Messaging.Outbox.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox.PostgreSQL",
				ContextName = "Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext",
				UnitOfWorkName = "OutboxQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Outbox.IOutboxQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.Outbox.PostgreSQL.OutboxQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Outbox.OutboxBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Outbox.OutboxBaseQueryEntity),
					typeof(Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext),
					typeof(Legion.ADF.Messaging.Outbox.IOutboxQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.Outbox Repositories",
				ModelNamespace = "Legion.ADF.Messaging.Outbox",
				EFNamespace = "Legion.ADF.Messaging.Outbox.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox.SqlServer",
				ContextName = "Legion.ADF.Messaging.Outbox.SqlServer.IOutboxDbContext",
				UnitOfWorkName = "OutboxUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Outbox.IOutboxRepository",
				RepositoryBase = "Legion.ADF.Messaging.Outbox.SqlServer.OutboxRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Outbox.OutboxBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Outbox.OutboxBaseEntity),
					typeof(Legion.ADF.Messaging.Outbox.SqlServer.IOutboxDbContext),
					typeof(Legion.ADF.Messaging.Outbox.IOutboxRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Messaging.Outbox QueryRepositories",
				ModelNamespace = "Legion.ADF.Messaging.Outbox",
				EFNamespace = "Legion.ADF.Messaging.Outbox.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Messaging.Outbox.SqlServer",
				ContextName = "Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext",
				UnitOfWorkName = "OutboxQueryUnitOfWork",
				IRepositry = "Legion.ADF.Messaging.Outbox.IOutboxQueryRepository",
				RepositoryBase = "Legion.ADF.Messaging.Outbox.SqlServer.OutboxQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Messaging.Outbox.OutboxBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Messaging.Outbox.OutboxBaseQueryEntity),
					typeof(Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext),
					typeof(Legion.ADF.Messaging.Outbox.IOutboxQueryRepository)
				]
			});
	}
}

