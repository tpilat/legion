using Legion.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Legion.Generators;

internal class RepositorySourceGenerator
{
	private const string IQueryGenericDefinition = "Legion.MessageBus.Messages.IQuery<>";

	public static (string Repository, List<(string? FileName, string? Code)> QueryHandlers) GeneratedSource(
		ITypeSymbol typeSymbol,
		List<QueryMetadata> queryMetadata,
		bool isEntityModel,
		string modelNamespace,
		string efNamespace,
		Database.Metamodel.DatabaseProviderType databaseProviderType,
		string ientityRepositry,
		string entityRepositoryBase,
		string contextName,
		bool isQueryModel,
		string unitOfWorkName)
	{
		var queries = GenereateQueries(queryMetadata, modelNamespace, efNamespace);
		var handlers = GenereateHandlers(typeSymbol, queryMetadata, modelNamespace, efNamespace, isQueryModel, unitOfWorkName);

		var sbRepository = new StringBuilder();

		sbRepository.AppendLine($@"using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace {GetRepositryFullNamespaceName(typeSymbol, modelNamespace, efNamespace)};

public partial class {typeSymbol.Name}Repository : {entityRepositoryBase}, {ientityRepositry}<{typeSymbol}>, {GetRepositryFullNamespaceName(typeSymbol, modelNamespace, modelNamespace)}.I{typeSymbol.Name}Repository
{{
	private readonly Lazy<Legion.ACL.IAccessControlManager<{typeSymbol}>?> _accessControlManager;

	private {contextName}? _context;

	public Legion.ACL.IAccessControlManager<{typeSymbol}>? AccessControlManager => _accessControlManager.Value;

	public {typeSymbol.Name}Repository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<{typeSymbol}>>());
	}}

	public IQueryable<{typeSymbol}> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<{typeSymbol}> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<{contextName}>(scopeContext)).{typeSymbol.Name};

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}}

	public IQueryable<{typeSymbol}> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<{typeSymbol}> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	{queries}{GenerateAddRemoveMethods(typeSymbol, isEntityModel, databaseProviderType)}}}");

		return (sbRepository.ToString(), handlers);
	}

	public static string GetRepositryFullNamespaceName(ITypeSymbol typeSymbol, string modelNamespace, string efNamespace)
		=> $"{efNamespace}{typeSymbol.ContainingNamespace.ToString()?.TrimPrefix(modelNamespace)}.Repositories";

	public static string GetQueryHandlerFullNamespaceName(ITypeSymbol typeSymbol, string modelNamespace, string efNamespace)
		=> $"{efNamespace}{typeSymbol.ContainingNamespace.ToString()?.TrimPrefix(modelNamespace).Replace("Queries", "QueryHandlers")}";

	public static string GetRepositryRelativeNamespaceName(ITypeSymbol typeSymbol, string baseNamespace)
		=> typeSymbol.ContainingNamespace.ToString()?.TrimPrefix($"{baseNamespace}.")!;

	public static string GetRepositryFullName(ITypeSymbol typeSymbol, string baseNamespace)
		=> $"{GetRepositryFullNamespaceName(typeSymbol, baseNamespace, baseNamespace)}.{typeSymbol.Name}Repository";

	private static string GenerateAddRemoveMethods(
		ITypeSymbol typeSymbol,
		bool isEntityModel,
		Database.Metamodel.DatabaseProviderType databaseProviderType)
	{
		if (!isEntityModel)
			return "";

		var sb = new StringBuilder();

		sb.AppendLine($@"
	public void Add(IScopeContext scopeContext, {typeSymbol} entity)
	{{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.{typeSymbol.Name}.Add(entity);
	}}

	public async Task AddAsync(
		IScopeContext scopeContext,
		{typeSymbol} entity,
		CancellationToken cancellationToken = default)
	{{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.{typeSymbol.Name}.AddAsync(entity, cancellationToken);
	}}

	public void AddRange(IScopeContext scopeContext, IEnumerable<{typeSymbol}> entities)
	{{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.{typeSymbol.Name}.AddRange(entities);
	}}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<{typeSymbol}> entities,
		CancellationToken cancellationToken = default)
	{{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.{typeSymbol.Name}.AddRangeAsync(entities, cancellationToken);
	}}

	public void Remove(IScopeContext scopeContext, {typeSymbol} entity)
	{{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.{typeSymbol.Name}.Remove(entity);
	}}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<{typeSymbol}> entities)
	{{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.{typeSymbol.Name}.RemoveRange(entities);
	}}");

		if (databaseProviderType == Database.Metamodel.DatabaseProviderType.PostgreSQL)
		{
			sb.AppendLine($@"

	public ulong BulkInsert(
		IEnumerable<{typeSymbol}> entities,
		bool allowCreateNewDbConnection = false)
	{{
		var tableInfo = TableInfoProvider.Get{typeSymbol.Name}TableInfo();
		string sql = $""COPY {{tableInfo.FullTableName}} ({{tableInfo.CommaSeparatedColumns}}) FROM STDIN (FORMAT BINARY)"";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($""{{nameof(npgsqlConnection)}} == null"");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);
		
			foreach (var entity in entities)
			{{
				writer.StartRow();
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					writer.Write(kvp.Value, columnTypes[kvp.Key]);
			}}

			result = writer.Complete();
		}}
		finally
		{{
			if (isNewConnection)
			{{
				writer?.Dispose();
				npgsqlConnection.Dispose();
			}}
		}}

		return result;
	}}

	public async Task<ulong> BulkInsertAsync(
		IEnumerable<{typeSymbol}> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{{
		var tableInfo = TableInfoProvider.Get{typeSymbol.Name}TableInfo();
		string sql = $""COPY {{tableInfo.FullTableName}} ({{tableInfo.CommaSeparatedColumns}}) FROM STDIN (FORMAT BINARY)"";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($""{{nameof(npgsqlConnection)}} == null"");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);

			foreach (var entity in entities)
			{{
				await writer.StartRowAsync(cancellationToken).ConfigureAwait(false);
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					await writer.WriteAsync(kvp.Value, columnTypes[kvp.Key], cancellationToken).ConfigureAwait(false);
			}}

			result = await writer.CompleteAsync(cancellationToken).ConfigureAwait(false);
		}}
		finally
		{{
			if (isNewConnection)
			{{
				if (writer != null)
					await writer.DisposeAsync();

				await npgsqlConnection.DisposeAsync();
			}}
		}}

		return result;
	}}");
		}
		else //************************ databaseProviderType == Database.Metamodel.DatabaseProviderType.SqlServer ************************
		{
			sb.AppendLine($@"

	public ulong BulkInsert(
		IEnumerable<{typeSymbol}> entities,
		bool allowCreateNewDbConnection = false)
	{{
		var tableInfo = TableInfoProvider.Get{typeSymbol.Name}TableInfo();

		var isNewConnection = false;
		var connection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (connection == null)
			Throw.InvalidOperationException($""{{nameof(connection)}} == null"");

		ulong result = 0;

		try
		{{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			var rows = entities.Select(e => e.ToDictionary()).ToList();
			var dataTable = tableInfo.ToDataTable(rows);

			using var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(connection);

			bulkCopy.DestinationTableName = tableInfo.FullTableName;

			foreach (var column in tableInfo.Columns)
				bulkCopy.ColumnMappings.Add(column.PropertyName, column.ColumnName);

			bulkCopy.WriteToServer(dataTable);
		}}
		finally
		{{
			if (isNewConnection)
				connection.Dispose();
		}}

		return result;
	}}");
		}

		return sb.ToString();
	}

	private static string GenereateQueries(
		List<QueryMetadata> queryMetadata,
		string modelNamespace,
		string efNamespace)
	{
		if (queryMetadata?.Any() != true)
			return "";

		var sb = new StringBuilder();

		var ignoredConstructorParams = new List<string> { "string repositoryIdentifier" };
		var ignoredConstructorParamNames = new List<string> { "repositoryIdentifier" };

		foreach (var metadata in queryMetadata)
		{
			for (int cp = 0; cp < metadata.ConstructorParameters.Count; cp++)
			{
				var constructorParameters = metadata.ConstructorParameters[cp].Where(x => !ignoredConstructorParams.Contains(x)).ToList();
				var sbParams = new StringBuilder();
				int i = 0;
				var count = constructorParameters.Count;

				foreach (var parameterString in constructorParameters)
				{
					i++;
					sbParams.AppendLine();
					sbParams.Append($"		{parameterString}{(i == count ? "" : ",")}");
				}

				var parameters = sbParams.ToString();
				parameters = string.IsNullOrWhiteSpace(parameters) ? "" : parameters;

				var constructorParameterNames = metadata.ConstructorParameterNames[cp].Where(x => !ignoredConstructorParamNames.Contains(x)).ToList();
				var sbParamNames = new StringBuilder();
				i = 0;
				count = constructorParameterNames.Count;

				foreach (var parameterName in constructorParameterNames)
				{
					i++;
					sbParamNames.AppendLine();
					sbParamNames.Append($"			{parameterName}{(i == count ? "" : ",")}");
				}

				var parameterNames = sbParamNames.ToString();
				parameterNames = string.IsNullOrWhiteSpace(parameterNames) ? "" : parameterNames;

				sb.AppendLine($@"
	public {metadata.IQueryFullName.Replace(efNamespace, modelNamespace)} {metadata.Query.Name}({parameters})
		=> new {metadata.QueryFullName.TrimPrefix($"{efNamespace}.")}(
			ConnectionProvider{(string.IsNullOrWhiteSpace(parameterNames) ? "" : $",{parameterNames}")});");
			}
		}

		var result = sb.ToString();
		result = string.IsNullOrWhiteSpace(result) ? "" : result;
		return result;
	}

	private static List<(string? FilePath, string? Code)> GenereateHandlers(
		ITypeSymbol typeSymbol,
		List<QueryMetadata> queryMetadata,
		string modelNamespace,
		string efNamespace,
		bool isQueryModel,
		string unitOfWorkName)
	{
		var result = new List<(string? FilePath, string? Code)>();

		if (queryMetadata?.Any() != true)
			return result;

		foreach (var metadata in queryMetadata)
		{
			var queryDefinition = metadata.Query.GetMembers().OfType<IMethodSymbol>()
			.Where(x => x.MethodKind == MethodKind.Constructor && x.DeclaredAccessibility == Accessibility.Public)
			.SelectMany(ms => ms.Parameters)
			.FirstOrDefault(ps => ps.Type.AllInterfaces.Any(ifc =>
				ifc.IsGenericType
				&& ifc.ConstructUnboundGenericType().ToString() == IQueryGenericDefinition));

			if (queryDefinition == null)
			{
				result.Add((null, null));
				continue;
			}

			var iqueryInterface = queryDefinition.Type.Interfaces.First(ifc => ifc.ConstructUnboundGenericType().ToString() == IQueryGenericDefinition);
			var queryResponseType = iqueryInterface.TypeArguments.First().ToString();

			//ConstructUnboundGenericType().ToString()
			//queryDefinition.Type.Interfaces.array[1].TypeArguments.array[0].ToString();

			var handlerResult = $@"using Legion;
using Legion.MessageBus.MessageHandlers;
using Legion.Model.Repositories;

namespace {GetQueryHandlerFullNamespaceName(metadata.Query, modelNamespace, modelNamespace)};

public class {queryDefinition.Type.Name}Handler : AsyncMessageHandlerBase<{queryDefinition.Type.ToString()}, {queryResponseType}>
{{
	public override async Task<IResult<{queryResponseType}>> HandleAsync(
		IInvocationContext invocationContext,
		{queryDefinition.Type.ToString()} query,
		Legion.Database.IConnectionProvider connectionProvider,
		CancellationToken cancellationToken = default)
	{{
		invocationContext = invocationContext.InvocationCreateNew();

		var result = new ResultBuilder<{queryResponseType}>();

		if (result.IsArgumentNull(invocationContext, query))
			return result.Build();

		if (result.IsArgumentNull(invocationContext, connectionProvider))
			return result.Build();

		var uowResult = connectionProvider.UnitOfWorkProvider.Create{(isQueryModel ? "Query" : "")}<I{unitOfWorkName}>(invocationContext);
		if (result.MergeHasError(uowResult))
			return result.Build();

		await using var uow = uowResult.Data!;
		var data = await uow.{typeSymbol.Name}Repository.{metadata.Query.Name}(query with {{ AsNoTracking = true }})
			.ToResultAsync(invocationContext, cancellationToken);

		return result.WithData(data).Build();
	}}
}}
";
			var dir = $"{queryDefinition.ContainingNamespace.ToString()?.Replace(efNamespace, "").Replace(modelNamespace, "").Replace(".", "\\").Replace("Queries", "QueryHandlers")}";
			result.Add(($"{dir}\\{queryDefinition.Type.Name}Handler.cs", handlerResult));
		}

		return result;
	}
}
