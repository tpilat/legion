using Legion.Generators;
//TODO RESX using Legion.ADF.Messaging.Resources;
using System.Globalization;

namespace Legion.ADF.Messaging.Outbox.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Messaging.Outbox.Generator";
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

