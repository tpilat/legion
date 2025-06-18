using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs.PostgreSQL.Model.Repositories;

public partial class EnvironmentInfoRepository : Legion.ADF.Logs.PostgreSQL.LogsRepositoryBase, Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.EnvironmentInfo>, Legion.ADF.Logs.Model.Repositories.IEnvironmentInfoRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.EnvironmentInfo>?> _accessControlManager;

	private Legion.ADF.Logs.PostgreSQL.ILogsDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.EnvironmentInfo>? AccessControlManager => _accessControlManager.Value;

	public EnvironmentInfoRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.EnvironmentInfo>>());
	}

	public IQueryable<Legion.ADF.Logs.Model.EnvironmentInfo> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Logs.Model.EnvironmentInfo> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Logs.PostgreSQL.ILogsDbContext>(scopeContext)).EnvironmentInfo;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Logs.Model.EnvironmentInfo> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Logs.Model.EnvironmentInfo> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Logs.Queries.EnvironmentInfo.IGetEnvironmentInfoById GetEnvironmentInfoById(
		Legion.ADF.Logs.Queries.EnvironmentInfo.GetEnvironmentInfoByIdQuery getEnvironmentInfoByIdQuery)
		=> new Queries.EnvironmentInfo.GetEnvironmentInfoById(
			ConnectionProvider,
			getEnvironmentInfoByIdQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Logs.Model.EnvironmentInfo entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.EnvironmentInfo.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Logs.Model.EnvironmentInfo entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.EnvironmentInfo.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Logs.Model.EnvironmentInfo> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.EnvironmentInfo.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Logs.Model.EnvironmentInfo> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.EnvironmentInfo.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Logs.Model.EnvironmentInfo entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.EnvironmentInfo.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Logs.Model.EnvironmentInfo> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.EnvironmentInfo.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Logs.Model.EnvironmentInfo> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetEnvironmentInfoTableInfo();
		string sql = $"COPY {tableInfo.FullTableName} ({tableInfo.CommaSeparatedColumns}) FROM STDIN (FORMAT BINARY)";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($"{nameof(npgsqlConnection)} == null");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);
		
			foreach (var entity in entities)
			{
				writer.StartRow();
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					writer.Write(kvp.Value, columnTypes[kvp.Key]);
			}

			result = writer.Complete();
		}
		finally
		{
			if (isNewConnection)
			{
				writer?.Dispose();
				npgsqlConnection.Dispose();
			}
		}

		return result;
	}

	public async Task<ulong> BulkInsertAsync(
		IEnumerable<Legion.ADF.Logs.Model.EnvironmentInfo> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetEnvironmentInfoTableInfo();
		string sql = $"COPY {tableInfo.FullTableName} ({tableInfo.CommaSeparatedColumns}) FROM STDIN (FORMAT BINARY)";

		var isNewConnection = false;
		var npgsqlConnection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (npgsqlConnection == null)
			Throw.InvalidOperationException($"{nameof(npgsqlConnection)} == null");

		Npgsql.NpgsqlBinaryImporter? writer = null;
		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			writer = npgsqlConnection.BeginBinaryImport(sql);

			foreach (var entity in entities)
			{
				await writer.StartRowAsync(cancellationToken).ConfigureAwait(false);
				var entityDict = entity.ToDictionary();

				foreach (var kvp in entityDict)
					await writer.WriteAsync(kvp.Value, columnTypes[kvp.Key], cancellationToken).ConfigureAwait(false);
			}

			result = await writer.CompleteAsync(cancellationToken).ConfigureAwait(false);
		}
		finally
		{
			if (isNewConnection)
			{
				if (writer != null)
					await writer.DisposeAsync();

				await npgsqlConnection.DisposeAsync();
			}
		}

		return result;
	}
}
