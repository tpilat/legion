using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer.Model.Repositories;

public partial class BlockedDomainEventTypeRepository : Legion.ADF.Messaging.DomainEvents.SqlServer.DomainEventsRepositoryBase, Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>, Legion.ADF.Messaging.DomainEvents.Model.Repositories.IBlockedDomainEventTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>?> _accessControlManager;

	private Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>? AccessControlManager => _accessControlManager.Value;

	public BlockedDomainEventTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>>());
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext>(scopeContext)).BlockedDomainEventType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType.IGetAllBlockedDomainEventTypes GetAllBlockedDomainEventTypes(
		Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType.GetAllBlockedDomainEventTypesQuery getAllBlockedDomainEventTypes)
		=> new Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType.GetAllBlockedDomainEventTypes(
			ConnectionProvider,
			getAllBlockedDomainEventTypes);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedDomainEventType.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.BlockedDomainEventType.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedDomainEventType.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.BlockedDomainEventType.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedDomainEventType.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.BlockedDomainEventType.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetBlockedDomainEventTypeTableInfo();

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
