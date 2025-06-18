using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.SqlServer.Model.Repositories;

public partial class OutboxMessageTypeRepository : Legion.ADF.Messaging.Outbox.SqlServer.OutboxRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>, Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxMessageTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.SqlServer.IOutboxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>? AccessControlManager => _accessControlManager.Value;

	public OutboxMessageTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.SqlServer.IOutboxDbContext>(scopeContext)).OutboxMessageType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.IGetAllOutboxMessageTypes GetAllOutboxMessageTypes(
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.GetAllOutboxMessageTypesQuery getAllOutboxMessageTypes)
		=> new Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.GetAllOutboxMessageTypes(
			ConnectionProvider,
			getAllOutboxMessageTypes);

	public Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.IGetOutboxMessageTypeByNamespace GetOutboxMessageTypeByNamespace(
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.GetOutboxMessageTypeByNamespaceQuery getOutboxMessageTypeByNamespace)
		=> new Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.GetOutboxMessageTypeByNamespace(
			ConnectionProvider,
			getOutboxMessageTypeByNamespace);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.Outbox.Model.OutboxMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OutboxMessageType.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.Outbox.Model.OutboxMessageType entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.OutboxMessageType.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OutboxMessageType.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.OutboxMessageType.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.Outbox.Model.OutboxMessageType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OutboxMessageType.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.OutboxMessageType.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetOutboxMessageTypeTableInfo();

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
