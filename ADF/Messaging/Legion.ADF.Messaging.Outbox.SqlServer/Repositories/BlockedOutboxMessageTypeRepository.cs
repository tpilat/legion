using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.SqlServer.Model.Repositories;

public partial class BlockedOutboxMessageTypeRepository : Legion.ADF.Messaging.Outbox.SqlServer.OutboxRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>, Legion.ADF.Messaging.Outbox.Model.Repositories.IBlockedOutboxMessageTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.SqlServer.IOutboxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>? AccessControlManager => _accessControlManager.Value;

	public BlockedOutboxMessageTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.SqlServer.IOutboxDbContext>(scopeContext)).BlockedOutboxMessageType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.IGetAllBlockedOutboxMessageTypes GetAllBlockedOutboxMessageTypes(
		Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetAllBlockedOutboxMessageTypesQuery getAllBlockedOutboxMessageTypes)
		=> new Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetAllBlockedOutboxMessageTypes(
			ConnectionProvider,
			getAllBlockedOutboxMessageTypes);

	public Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.IGetBlockedOutboxMessageTypesByNamespaces GetBlockedOutboxMessageTypesByNamespaces(
		Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetBlockedOutboxMessageTypesByNamespacesQuery GetBlockedOutboxMessageTypesByNamespaces)
		=> new Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetBlockedOutboxMessageTypesByNamespaces(
			ConnectionProvider,
			GetBlockedOutboxMessageTypesByNamespaces);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedOutboxMessageType.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.BlockedOutboxMessageType.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedOutboxMessageType.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.BlockedOutboxMessageType.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedOutboxMessageType.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedOutboxMessageType.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetBlockedOutboxMessageTypeTableInfo();

		var isNewConnection = false;
		var connection = allowCreateNewDbConnection
			? GetOrCreateNewDbConnection(out isNewConnection)
			: GetDbConnection();

		if (connection == null)
			Throw.InvalidOperationException($"{nameof(connection)} == null");

		ulong result = 0;

		try
		{
			var columnTypes = tableInfo.Columns.ToDictionary(x => x.PropertyName, x => x.DatabaseType);

			var rows = entities.Select(e => e.ToDictionary()).ToList();
			var dataTable = tableInfo.ToDataTable(rows);

			using var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(connection);

			bulkCopy.DestinationTableName = tableInfo.FullTableName;

			foreach (var column in tableInfo.Columns)
				bulkCopy.ColumnMappings.Add(column.PropertyName, column.ColumnName);

			bulkCopy.WriteToServer(dataTable);
		}
		finally
		{
			if (isNewConnection)
				connection.Dispose();
		}

		return result;
	}
}
