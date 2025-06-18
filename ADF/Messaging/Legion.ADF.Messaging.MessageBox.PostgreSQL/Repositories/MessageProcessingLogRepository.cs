using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories;

public partial class MessageProcessingLogRepository : Legion.ADF.Messaging.MessageBox.PostgreSQL.MessageBoxRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageProcessingLogRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog>? AccessControlManager => _accessControlManager.Value;

	public MessageProcessingLogRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext>(scopeContext)).MessageProcessingLog;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageProcessingLog.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.MessageProcessingLog.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageProcessingLog.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.MessageProcessingLog.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageProcessingLog.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.MessageProcessingLog.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetMessageProcessingLogTableInfo();
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
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetMessageProcessingLogTableInfo();
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
