using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.SqlServer.Model.Repositories;

public partial class BlockedInboxMessageTypeRepository : Legion.ADF.Messaging.Inbox.SqlServer.InboxRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>, Legion.ADF.Messaging.Inbox.Model.Repositories.IBlockedInboxMessageTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>? AccessControlManager => _accessControlManager.Value;

	public BlockedInboxMessageTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext>(scopeContext)).BlockedInboxMessageType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.IGetAllBlockedInboxMessageTypes GetAllBlockedInboxMessageTypes(
		Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetAllBlockedInboxMessageTypesQuery getAllBlockedInboxMessageTypes)
		=> new Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetAllBlockedInboxMessageTypes(
			ConnectionProvider,
			getAllBlockedInboxMessageTypes);

	public Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.IGetBlockedInboxMessageTypesByNamespaces GetBlockedInboxMessageTypesByNamespaces(
		Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetBlockedInboxMessageTypesByNamespacesQuery GetBlockedInboxMessageTypesByNamespaces)
		=> new Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetBlockedInboxMessageTypesByNamespaces(
			ConnectionProvider,
			GetBlockedInboxMessageTypesByNamespaces);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedInboxMessageType.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.BlockedInboxMessageType.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedInboxMessageType.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.BlockedInboxMessageType.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedInboxMessageType.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedInboxMessageType.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetBlockedInboxMessageTypeTableInfo();

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
