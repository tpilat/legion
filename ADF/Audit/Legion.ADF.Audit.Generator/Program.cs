using Legion.Generators;
//TODO RESX using Legion.ADF.Audit.Resources;
using System.Globalization;

namespace Legion.ADF.Audit.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Audit.Generator";
		//var targetProject = "Legion.ADF.Audit.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.Audit.Resources.Localizers).Assembly,
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
				RepoName = "Legion.ADF.Audit Repositories",
				ModelNamespace = "Legion.ADF.Audit",
				EFNamespace = "Legion.ADF.Audit.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit.PostgreSQL",
				ContextName = "Legion.ADF.Audit.PostgreSQL.IAuditDbContext",
				UnitOfWorkName = "AuditUnitOfWork",
				IRepositry = "Legion.ADF.Audit.IAuditRepository",
				RepositoryBase = "Legion.ADF.Audit.PostgreSQL.AuditRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Audit.AuditBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Audit.AuditBaseEntity),
					typeof(Legion.ADF.Audit.PostgreSQL.IAuditDbContext),
					typeof(Legion.ADF.Audit.IAuditRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Audit QueryRepositories",
				ModelNamespace = "Legion.ADF.Audit",
				EFNamespace = "Legion.ADF.Audit.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit.PostgreSQL",
				ContextName = "Legion.ADF.Audit.PostgreSQL.IAuditQueryDbContext",
				UnitOfWorkName = "AuditQueryUnitOfWork",
				IRepositry = "Legion.ADF.Audit.IAuditQueryRepository",
				RepositoryBase = "Legion.ADF.Audit.PostgreSQL.AuditQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Audit.AuditBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Audit.AuditBaseQueryEntity),
					typeof(Legion.ADF.Audit.PostgreSQL.IAuditQueryDbContext),
					typeof(Legion.ADF.Audit.IAuditQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Audit Repositories",
				ModelNamespace = "Legion.ADF.Audit",
				EFNamespace = "Legion.ADF.Audit.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit.SqlServer",
				ContextName = "Legion.ADF.Audit.SqlServer.IAuditDbContext",
				UnitOfWorkName = "AuditUnitOfWork",
				IRepositry = "Legion.ADF.Audit.IAuditRepository",
				RepositoryBase = "Legion.ADF.Audit.SqlServer.AuditRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Audit.AuditBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Audit.AuditBaseEntity),
					typeof(Legion.ADF.Audit.SqlServer.IAuditDbContext),
					typeof(Legion.ADF.Audit.IAuditRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Audit QueryRepositories",
				ModelNamespace = "Legion.ADF.Audit",
				EFNamespace = "Legion.ADF.Audit.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Audit.SqlServer",
				ContextName = "Legion.ADF.Audit.SqlServer.IAuditQueryDbContext",
				UnitOfWorkName = "AuditQueryUnitOfWork",
				IRepositry = "Legion.ADF.Audit.IAuditQueryRepository",
				RepositoryBase = "Legion.ADF.Audit.SqlServer.AuditQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Audit.AuditBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Audit.AuditBaseQueryEntity),
					typeof(Legion.ADF.Audit.SqlServer.IAuditQueryDbContext),
					typeof(Legion.ADF.Audit.IAuditQueryRepository)
				]
			});
	}
}

