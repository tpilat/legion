using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs.PostgreSQL.Model.Repositories;

public partial class RemoteResponseRepository : Legion.ADF.Logs.PostgreSQL.LogsRepositoryBase, Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.RemoteResponse>, Legion.ADF.Logs.Model.Repositories.IRemoteResponseRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteResponse>?> _accessControlManager;

	private Legion.ADF.Logs.PostgreSQL.ILogsDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteResponse>? AccessControlManager => _accessControlManager.Value;

	public RemoteResponseRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteResponse>>());
	}

	public IQueryable<Legion.ADF.Logs.Model.RemoteResponse> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Logs.Model.RemoteResponse> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Logs.PostgreSQL.ILogsDbContext>(scopeContext)).RemoteResponse;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Logs.Model.RemoteResponse> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Logs.Model.RemoteResponse> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Logs.Model.RemoteResponse entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RemoteResponse.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Logs.Model.RemoteResponse entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.RemoteResponse.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Logs.Model.RemoteResponse> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RemoteResponse.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Logs.Model.RemoteResponse> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.RemoteResponse.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Logs.Model.RemoteResponse entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RemoteResponse.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Logs.Model.RemoteResponse> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.RemoteResponse.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Logs.Model.RemoteResponse> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetRemoteResponseTableInfo();
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
		IEnumerable<Legion.ADF.Logs.Model.RemoteResponse> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetRemoteResponseTableInfo();
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
