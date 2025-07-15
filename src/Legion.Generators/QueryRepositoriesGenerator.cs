using Legion.Extensions;
using System.Text;
using System.Xml.Schema;

namespace Legion.Generators;

public class QueryRepositoriesGenerator
{
	private readonly static UTF8Encoding _encoding = new();

	public static void Generate(string solutionDirectoryPath, RepoGeneratorOptions generatorOptions)
	{
		Console.WriteLine($"NOTICE: generating {generatorOptions.RepoName}");

		if (string.IsNullOrWhiteSpace(solutionDirectoryPath))
			throw new ArgumentNullException(nameof(solutionDirectoryPath));

		var ignoredEntities = new List<string>
		{
		};

		var modelSourceParser = new ModelSourceParser(ignoredEntities);
		ParseModel(modelSourceParser, generatorOptions);

		var querySourceGenerator = new QuerySourceParser(modelSourceParser.QueryEntityModel);
		GenerateQueries(querySourceGenerator, generatorOptions);

		var iUnitOfWorkSB = new StringBuilder();
		var iUnitOfWorkSBFactory = new StringBuilder();

		iUnitOfWorkSB.Append($@"namespace {generatorOptions.ModelNamespace};

public partial interface I{generatorOptions.UnitOfWorkName} : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{{");

		iUnitOfWorkSBFactory.Append($@"namespace {generatorOptions.ModelNamespace};

public partial interface I{generatorOptions.UnitOfWorkName}Factory : Legion.Model.Repositories.IQueryUnitOfWorkFactory<I{generatorOptions.UnitOfWorkName}>
{{
}}
");

		var unitOfWorkSB = new StringBuilder();
		unitOfWorkSB.Append($@"using Legion;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace {generatorOptions.EFNamespace};

internal partial class {generatorOptions.UnitOfWorkName} : {generatorOptions.ModelNamespace}.I{generatorOptions.UnitOfWorkName}, Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork, Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{{
	private bool _isInternalConnectionProvider;
	private bool _disposed;

#if TRACK_OBJECTS
	public Guid Id{generatorOptions.UnitOfWorkName} {{ get; }}
#endif

	public IEFConnectionProvider ConnectionProvider {{ get; }}
	Legion.Database.IConnectionProvider Legion.Model.Repositories.IQueryUnitOfWork.ConnectionProvider => ConnectionProvider;
	System.IServiceProvider Legion.Model.Repositories.IQueryUnitOfWork.ServiceProvider => ConnectionProvider.ServiceProvider;
	
	public {generatorOptions.UnitOfWorkName}(IEFConnectionProvider connectionProvider)
	{{
#if TRACK_OBJECTS
		Id{generatorOptions.UnitOfWorkName} = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, Id{generatorOptions.UnitOfWorkName}.ToString());
#endif

		Throw.IfArgumentNull(connectionProvider);

		ConnectionProvider = connectionProvider;
		_isInternalConnectionProvider = false; //disposed by caller
	}}

	public {generatorOptions.UnitOfWorkName}(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
	{{
#if TRACK_OBJECTS
		Id{generatorOptions.UnitOfWorkName} = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, Id{generatorOptions.UnitOfWorkName}.ToString());
#endif

		Throw.IfArgumentNull(dbUnitOfWork);

		ConnectionProvider = dbUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbUnitOfWork
	}}

	public {generatorOptions.UnitOfWorkName}(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
	{{
#if TRACK_OBJECTS
		Id{generatorOptions.UnitOfWorkName} = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, Id{generatorOptions.UnitOfWorkName}.ToString());
#endif

		Throw.IfArgumentNull(dbQueryUnitOfWork);

		ConnectionProvider = dbQueryUnitOfWork.ConnectionProvider;
		_isInternalConnectionProvider = false; //disposed by dbQueryUnitOfWork
	}}

	public {generatorOptions.UnitOfWorkName}(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
	{{
#if TRACK_OBJECTS
		Id{generatorOptions.UnitOfWorkName} = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, Id{generatorOptions.UnitOfWorkName}.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithNewTransaction(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}}

	public {generatorOptions.UnitOfWorkName}(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
	{{
#if TRACK_OBJECTS
		Id{generatorOptions.UnitOfWorkName} = Legion.GlobalContext.Instance.NewGuid();
		Trackers.ObjectLifetimeTracker.Track(this, Id{generatorOptions.UnitOfWorkName}.ToString());
#endif

		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(connectionStirng);

		var connectionProviderFactory = serviceProvider.GetRequiredService<IEFConnectionProviderFactory>();
		ConnectionProvider = connectionProviderFactory.CreateWithoutTransaction(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
		_isInternalConnectionProvider = true;
	}}

	protected {generatorOptions.ContextName} GetContext(IScopeContext scopeContext)
		=> ConnectionProvider.GetOrCreateDbContext<{generatorOptions.ContextName}>(scopeContext);
");

		bool first = true;
		foreach (var kvp in modelSourceParser.QueryEntityModel.OrderBy(x => x.Key.Name))
		{
			if (generatorOptions.IgnoredTypes?.Contains(kvp.Key.ToString()!) == true)
				continue;

			if (0 < kvp.Value.Count)
			{
				foreach (var queryMetadata in kvp.Value)
				{
					var iquerySource = IQuerySourceGenerator.GeneratedSource(queryMetadata.Query, queryMetadata, generatorOptions.ModelNamespace, generatorOptions.EFNamespace);

					var iqueryFilePath = queryMetadata.SourceFilePath.Replace(generatorOptions.EFNamespace, generatorOptions.ModelNamespace);
					var iqueryFileName = iqueryFilePath[(iqueryFilePath.LastIndexOf("\\") + 1)..];
					iqueryFilePath = $"{iqueryFilePath.TrimPostfix(iqueryFileName)}I{iqueryFileName}";
					var iqueryDirPath = Path.GetDirectoryName(iqueryFilePath);

					if (!Directory.Exists(iqueryDirPath))
						Directory.CreateDirectory(iqueryDirPath!);

					FileWriter.WriteAllText(iqueryFilePath, iquerySource, _encoding);
				}
			}

			var irepositorySource = IRepositorySourceGenerator.GeneratedSource(
				kvp.Key,
				kvp.Value,
				generatorOptions.ModelNamespace,
				generatorOptions.EFNamespace,
				generatorOptions.IRepositry);

			var irepositoryRelativeName = $"{($"{kvp.Key.ContainingNamespace.ToString().TrimPrefix(generatorOptions.ModelNamespace).TrimPrefix(".Model")}\\Repositories".Replace(".", "\\"))}\\I{kvp.Key.Name}Repository.cs";
			var irepositoryFilePath = Path.Combine(generatorOptions.ModelProjectPath, irepositoryRelativeName.TrimPrefix("\\"));
			var irepositoryDirPath = Path.GetDirectoryName(irepositoryFilePath);

			if (!Directory.Exists(irepositoryDirPath))
				Directory.CreateDirectory(irepositoryDirPath!);

			FileWriter.WriteAllText(irepositoryFilePath, irepositorySource, _encoding);

			var repostirySource = RepositorySourceGenerator.GeneratedSource(
				kvp.Key,
				kvp.Value,
				false,
				generatorOptions.ModelNamespace,
				generatorOptions.EFNamespace,
				generatorOptions.DatabaseProviderType,
				generatorOptions.IRepositry,
				generatorOptions.RepositoryBase,
				generatorOptions.ContextName,
				true,
				generatorOptions.UnitOfWorkName);

			var repositoryRelativeName = $"{($"{kvp.Key.ContainingNamespace.ToString().TrimPrefix(generatorOptions.ModelNamespace).TrimPrefix(".Model")}\\Repositories".Replace(".", "\\"))}\\{kvp.Key.Name}Repository.cs";
			var repositoryFilePath = Path.Combine(generatorOptions.SQLProjectPath, repositoryRelativeName.TrimPrefix("\\"));
			var repositoryDirPath = Path.GetDirectoryName(repositoryFilePath);

			if (!Directory.Exists(repositoryDirPath))
				Directory.CreateDirectory(repositoryDirPath!);

			FileWriter.WriteAllText(repositoryFilePath, repostirySource.Repository, _encoding);

			foreach (var (handlerRelativeFilePath, code) in repostirySource.QueryHandlers)
			{
				if (string.IsNullOrWhiteSpace(handlerRelativeFilePath) || string.IsNullOrWhiteSpace(code))
					continue;

				var handlerFilePath = Path.Combine(generatorOptions.SQLProjectPath, handlerRelativeFilePath.TrimPrefix("\\"));
				var handlerDirPath = Path.GetDirectoryName(handlerFilePath);

				if (!Directory.Exists(handlerDirPath))
					Directory.CreateDirectory(handlerDirPath!);

				FileWriter.WriteAllText(handlerFilePath, code, _encoding);
			}

			iUnitOfWorkSB.AppendLine();
			iUnitOfWorkSB.AppendLine($@"	{IRepositorySourceGenerator.GetRepositryRelativeNamespaceName(kvp.Key)}.I{kvp.Key.Name}Repository {kvp.Key.Name}Repository {{ get; }}");

			if (!first)
				unitOfWorkSB.AppendLine();

			unitOfWorkSB.AppendLine($@"
	private {IRepositorySourceGenerator.GetRepositryRelativeNamespaceName(kvp.Key)}.I{kvp.Key.Name}Repository? {kvp.Key.Name.FirstToLower(true)};
	public {IRepositorySourceGenerator.GetRepositryRelativeNamespaceName(kvp.Key)}.I{kvp.Key.Name}Repository {kvp.Key.Name}Repository
		=> {kvp.Key.Name.FirstToLower(true)} ??= new {RepositorySourceGenerator.GetRepositryFullNamespaceName(kvp.Key, generatorOptions.ModelNamespace, generatorOptions.EFNamespace)}.{kvp.Key.Name}Repository(ConnectionProvider);");

			first = false;
		}

		unitOfWorkSB.AppendLine(@$"
	public async ValueTask DisposeAsync()
	{{
		if (_disposed)
			return;

		_disposed = true;

		await DisposeAsyncCoreAsync().ConfigureAwait(false);

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}}

	private async ValueTask DisposeAsyncCoreAsync()
	{{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, Id{generatorOptions.UnitOfWorkName}.ToString());
#endif

		if (_isInternalConnectionProvider && ConnectionProvider != null)
		{{
			await ConnectionProvider.DisposeAsync();
		}}
	}}

	private void Dispose(bool disposing)
	{{
		if (_disposed)
			return;

		_disposed = true;

		if (disposing)
		{{
#if TRACK_OBJECTS
		Trackers.ObjectLifetimeTracker.SetDisposed(this, Id{generatorOptions.UnitOfWorkName}.ToString());
#endif

			if (_isInternalConnectionProvider)
			{{
				ConnectionProvider?.Dispose();
			}}
		}}
	}}

	public void Dispose()
	{{
		Dispose(true);
		GC.SuppressFinalize(this);
	}}");

		var unitOfWorkSBFactory = new StringBuilder();
		unitOfWorkSBFactory.Append($@"namespace {generatorOptions.EFNamespace};

public partial class {generatorOptions.UnitOfWorkName}Factory : I{generatorOptions.UnitOfWorkName}Factory, Legion.Model.Repositories.IQueryUnitOfWorkFactory<I{generatorOptions.UnitOfWorkName}>
{{
	public I{generatorOptions.UnitOfWorkName} Create(Legion.Database.IConnectionProvider connectionProvider)
	{{
		if (connectionProvider is not Legion.EntityFrameworkCore.IEFConnectionProvider efConnectionProvider)
		{{
			Legion.Throw.InvalidOperationException($""The provided {{nameof(Legion.Database.IConnectionProvider)}} is not an instance of {{nameof(Legion.EntityFrameworkCore.IEFConnectionProvider)}}"");
			return null!;
		}}

		return new {generatorOptions.UnitOfWorkName}(efConnectionProvider);
	}}

	public I{generatorOptions.UnitOfWorkName} Create(Legion.Model.Repositories.IUnitOfWork unitOfWork)
	{{
		if (unitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork dbUnitOfWork)
		{{
			Legion.Throw.InvalidOperationException($""The provided {{nameof(Legion.Model.Repositories.IUnitOfWork)}} is not an instance of {{nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbUnitOfWork)}}"");
			return null!;
		}}

		return new {generatorOptions.UnitOfWorkName}(dbUnitOfWork);
	}}

	public I{generatorOptions.UnitOfWorkName} Create(Legion.Model.Repositories.IQueryUnitOfWork queryUnitOfWork)
	{{
		if (queryUnitOfWork is not Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork dbQueryUnitOfWork)
		{{
			Legion.Throw.InvalidOperationException($""The provided {{nameof(Legion.Model.Repositories.IQueryUnitOfWork)}} is not an instance of {{nameof(Legion.EntityFrameworkCore.Model.Repositories.IDbQueryUnitOfWork)}}"");
			return null!;
		}}

		return new {generatorOptions.UnitOfWorkName}(dbQueryUnitOfWork);
	}}

	public I{generatorOptions.UnitOfWorkName} Create(
		IServiceProvider serviceProvider,
		string connectionStirng,
		System.Data.IsolationLevel? isolationLevel,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new {generatorOptions.UnitOfWorkName}(
			serviceProvider,
			connectionStirng,
			isolationLevel,
			allowLocking,
			createAuditEntryStore);

	public I{generatorOptions.UnitOfWorkName} CreateWithoutTransaction(
		IServiceProvider serviceProvider,
		string connectionStirng,
		bool? allowLocking,
		bool createAuditEntryStore)
		=> new {generatorOptions.UnitOfWorkName}(
			serviceProvider,
			connectionStirng,
			allowLocking,
			createAuditEntryStore);
}}
");

		iUnitOfWorkSB.AppendLine("}");
		unitOfWorkSB.AppendLine("}");

		var iUnitOfWorkSource = iUnitOfWorkSB.ToString();
		var iUnitOfWorkFactorySource = iUnitOfWorkSBFactory.ToString();
		var unitOfWorkSource = unitOfWorkSB.ToString();
		var unitOfWorkFactorySource = unitOfWorkSBFactory.ToString();

		var iUnitOfWorkFilePath = Path.Combine(generatorOptions.ModelProjectPath, $"I{generatorOptions.UnitOfWorkName}.cs");
		var iUnitOfWorkDirPath = Path.GetDirectoryName(iUnitOfWorkFilePath);

		if (!Directory.Exists(iUnitOfWorkDirPath))
			Directory.CreateDirectory(iUnitOfWorkDirPath!);

		FileWriter.WriteAllText(iUnitOfWorkFilePath, iUnitOfWorkSource, _encoding);

		var iUnitOfWorkFactoryFilePath = Path.Combine(generatorOptions.ModelProjectPath, $"I{generatorOptions.UnitOfWorkName}Factory.cs");
		var iUnitOfWorkFactoryDirPath = Path.GetDirectoryName(iUnitOfWorkFactoryFilePath);

		if (!Directory.Exists(iUnitOfWorkFactoryDirPath))
			Directory.CreateDirectory(iUnitOfWorkFactoryDirPath!);

		FileWriter.WriteAllText(iUnitOfWorkFactoryFilePath, iUnitOfWorkFactorySource, _encoding);

		var unitOfWorkFilePath = Path.Combine(generatorOptions.SQLProjectPath, $"{generatorOptions.UnitOfWorkName}.cs");
		var unitOfWorkDirPath = Path.GetDirectoryName(unitOfWorkFilePath);

		if (!Directory.Exists(unitOfWorkDirPath))
			Directory.CreateDirectory(unitOfWorkDirPath!);

		FileWriter.WriteAllText(unitOfWorkFilePath, unitOfWorkSource, _encoding);

		var unitOfWorkFactoryFilePath = Path.Combine(generatorOptions.SQLProjectPath, $"{generatorOptions.UnitOfWorkName}Factory.cs");
		var unitOfWorkFactoryDirPath = Path.GetDirectoryName(unitOfWorkFactoryFilePath);

		if (!Directory.Exists(unitOfWorkFactoryDirPath))
			Directory.CreateDirectory(unitOfWorkFactoryDirPath!);

		FileWriter.WriteAllText(unitOfWorkFactoryFilePath, unitOfWorkFactorySource, _encoding);

		Console.WriteLine($"SUCCESS: {generatorOptions.RepoName}");
	}

	static void ParseModel(ModelSourceParser modelSourceParser, RepoGeneratorOptions generatorOptions)
	{
		var dir = generatorOptions.ModelProjectPath;
		var sourcesDict = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories)
			.ToDictionary(x => x, x => File.ReadAllText(x, _encoding));

		foreach (var kvp in sourcesDict)
		{
			modelSourceParser.CurrentSourceFilePath = kvp.Key;

			var modelCompilation = Compilator.CreateCompilation(
				kvp.Value,
				typeof(System.Threading.Tasks.Task),
				typeof(Legion.Model.IQueryEntity));

			var newModelCompilation = Compilator.RunGenerators(modelCompilation, out _, modelSourceParser);
		}
	}

	static void GenerateQueries(QuerySourceParser querySourceGenerator, RepoGeneratorOptions generatorOptions)
	{
		var dir = generatorOptions.SQLProjectPath;

		if (!Directory.Exists(dir))
			return;

		var sourcesDict = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories)
			.ToDictionary(x => x, x => File.ReadAllText(x, _encoding));

		foreach (var kvp in sourcesDict)
		{
			querySourceGenerator.CurrentSourceFilePath = kvp.Key;
			querySourceGenerator.CurrentQueryMetadata = null!;

			var types = new List<Type>
			{
				typeof(System.Threading.Tasks.Task),
				typeof(Legion.Model.IQueryEntity),
				typeof(Legion.EntityFrameworkCore.Queries.QueryDefinition<,,,>)
			};

			if (0 < generatorOptions.QueryCompileTypes?.Count)
				types.AddRange(generatorOptions.QueryCompileTypes);

			var queryCompilation = Compilator.CreateCompilation(
				kvp.Value,
				types.ToArray());

			var newQueryCompilation = Compilator.RunGenerators(queryCompilation, out _, querySourceGenerator);

			//var newFile =
			//	newQueryCompilation.SyntaxTrees
			//		.FirstOrDefault(x => Path.GetFileName(x.FilePath).EndsWith(".GEN.cs"));

			//if (newFile == null)
			//	continue;

			//var source = newFile.GetText().ToString();

			//var targetFilePath = kvp.Key.Replace(".PostgreSQL", ".Abstractions");
			//var fileName = targetFilePath[(targetFilePath.LastIndexOf("\\") + 1)..];
			//targetFilePath = $"{targetFilePath.TrimPostfix(fileName)}I{fileName}";
			//var dirPath = Path.GetDirectoryName(targetFilePath);

			//if (!Directory.Exists(dirPath))
			//	Directory.CreateDirectory(dirPath!);

			//FileWriter.WriteAllText(targetFilePath, source, _encosing);
		}
	}
}
