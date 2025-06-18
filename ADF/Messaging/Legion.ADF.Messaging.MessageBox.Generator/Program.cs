using Legion.Generators;
//TODO RESX using Legion.ADF.Messaging.Resources;
using System.Globalization;

namespace Legion.ADF.Messaging.MessageBox.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Messaging.MessageBox.Generator";
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
	}
}

