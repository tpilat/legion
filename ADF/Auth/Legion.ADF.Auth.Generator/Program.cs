using Legion.Generators;
//TODO RESX using Legion.ADF.Auth.Resources;
using System.Globalization;

namespace Legion.ADF.Auth.Generator;

internal class Program
{
	static void Main(string[] args)
	{
		//TODO RESX
		//Console.WriteLine("NOTICE: generating Resources");

		//var thisProjectName = "Legion.ADF.Auth.Generator";
		//var targetProject = "Legion.ADF.Auth.Resources";
		//var defaultCulture = CultureInfo.GetCultureInfo("sk");

		//var entryAssemblyLocation = System.Reflection.Assembly.GetEntryAssembly()?.Location;
		//var solutionRootFolder = entryAssemblyLocation?[..entryAssemblyLocation.IndexOf(thisProjectName)] ?? throw new InvalidOperationException("No solutionRootFolder");
		//var targetProjectDirectory = Path.Combine(solutionRootFolder, targetProject);

		//Legion.ResourcesGenerator.Generator.GenerateResources(
		//	targetProjectDirectory,
		//	targetProject,
		//	false,
		//	typeof(Legion.ADF.Auth.Resources.Localizers).Assembly,
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
				RepoName = "Legion.ADF.Auth Repositories",
				ModelNamespace = "Legion.ADF.Auth",
				EFNamespace = "Legion.ADF.Auth.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth.PostgreSQL",
				ContextName = "Legion.ADF.Auth.PostgreSQL.IAuthDbContext",
				UnitOfWorkName = "AuthUnitOfWork",
				IRepositry = "Legion.ADF.Auth.IAuthRepository",
				RepositoryBase = "Legion.ADF.Auth.PostgreSQL.AuthRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Auth.AuthBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Auth.AuthBaseEntity),
					typeof(Legion.ADF.Auth.PostgreSQL.IAuthDbContext),
					typeof(Legion.ADF.Auth.IAuthRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Auth QueryRepositories",
				ModelNamespace = "Legion.ADF.Auth",
				EFNamespace = "Legion.ADF.Auth.PostgreSQL",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.PostgreSQL,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth.PostgreSQL",
				ContextName = "Legion.ADF.Auth.PostgreSQL.IAuthQueryDbContext",
				UnitOfWorkName = "AuthQueryUnitOfWork",
				IRepositry = "Legion.ADF.Auth.IAuthQueryRepository",
				RepositoryBase = "Legion.ADF.Auth.PostgreSQL.AuthQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Auth.AuthBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Auth.AuthBaseQueryEntity),
					typeof(Legion.ADF.Auth.PostgreSQL.IAuthQueryDbContext),
					typeof(Legion.ADF.Auth.IAuthQueryRepository)
				]
			});

		EntityRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Auth Repositories",
				ModelNamespace = "Legion.ADF.Auth",
				EFNamespace = "Legion.ADF.Auth.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth.SqlServer",
				ContextName = "Legion.ADF.Auth.SqlServer.IAuthDbContext",
				UnitOfWorkName = "AuthUnitOfWork",
				IRepositry = "Legion.ADF.Auth.IAuthRepository",
				RepositoryBase = "Legion.ADF.Auth.SqlServer.AuthRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Auth.AuthBaseEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Auth.AuthBaseEntity),
					typeof(Legion.ADF.Auth.SqlServer.IAuthDbContext),
					typeof(Legion.ADF.Auth.IAuthRepository)
				]
			});

		QueryRepositoriesGenerator.Generate(
			solutionDirectoryPath,
			new RepoGeneratorOptions
			{
				RepoName = "Legion.ADF.Auth QueryRepositories",
				ModelNamespace = "Legion.ADF.Auth",
				EFNamespace = "Legion.ADF.Auth.SqlServer",
				ModelProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth",
				DatabaseProviderType = Database.Metamodel.DatabaseProviderType.SqlServer,
				SQLProjectPath = $@"{solutionDirectoryPath}\Legion.ADF.Auth.SqlServer",
				ContextName = "Legion.ADF.Auth.SqlServer.IAuthQueryDbContext",
				UnitOfWorkName = "AuthQueryUnitOfWork",
				IRepositry = "Legion.ADF.Auth.IAuthQueryRepository",
				RepositoryBase = "Legion.ADF.Auth.SqlServer.AuthQueryRepositoryBase",
				UoWObsoletePrefix = "MM_Mod",
				IgnoredTypes =
				[
					"Legion.ADF.Auth.AuthBaseQueryEntity"
				],
				QueryCompileTypes =
				[
					typeof(Legion.ADF.Auth.AuthBaseQueryEntity),
					typeof(Legion.ADF.Auth.SqlServer.IAuthQueryDbContext),
					typeof(Legion.ADF.Auth.IAuthQueryRepository)
				]
			});
	}
}

