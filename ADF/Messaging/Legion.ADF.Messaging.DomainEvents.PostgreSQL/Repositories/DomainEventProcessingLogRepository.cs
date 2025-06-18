using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL.Model.Repositories;

public partial class DomainEventProcessingLogRepository : Legion.ADF.Messaging.DomainEvents.PostgreSQL.DomainEventsRepositoryBase, Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>, Legion.ADF.Messaging.DomainEvents.Model.Repositories.IDomainEventProcessingLogRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>?> _accessControlManager;

	private Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>? AccessControlManager => _accessControlManager.Value;

	public DomainEventProcessingLogRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>>());
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsDbContext>(scopeContext)).DomainEventProcessingLog;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog.IGetAllDomainEventProcessingLogsByIdDomainEvent GetAllDomainEventProcessingLogsByIdDomainEvent(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEventQuery getAllDomainEventProcessingLogsByIdDomainEventQuery)
		=> new Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEvent(
			ConnectionProvider,
			getAllDomainEventProcessingLogsByIdDomainEventQuery);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEventProcessingLog.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.DomainEventProcessingLog.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEventProcessingLog.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.DomainEventProcessingLog.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEventProcessingLog.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEventProcessingLog.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetDomainEventProcessingLogTableInfo();
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
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetDomainEventProcessingLogTableInfo();
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
