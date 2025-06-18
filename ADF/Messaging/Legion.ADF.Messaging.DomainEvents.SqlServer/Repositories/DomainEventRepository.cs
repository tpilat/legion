using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.DomainEvents.SqlServer.Model.Repositories;

public partial class DomainEventRepository : Legion.ADF.Messaging.DomainEvents.SqlServer.DomainEventsRepositoryBase, Legion.ADF.Messaging.DomainEvents.IDomainEventsRepository<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>, Legion.ADF.Messaging.DomainEvents.Model.Repositories.IDomainEventRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>?> _accessControlManager;

	private Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>? AccessControlManager => _accessControlManager.Value;

	public DomainEventRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>>());
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext>(scopeContext)).DomainEvent;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.IExistsDomainEventByIdDomainEvent ExistsDomainEventByIdDomainEvent(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.ExistsDomainEventByIdDomainEventQuery existsDomainEventByIdDomainEvent)
		=> new Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.ExistsDomainEventByIdDomainEvent(
			ConnectionProvider,
			existsDomainEventByIdDomainEvent);

	public Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.IGetDomainEventById GetDomainEventById(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetDomainEventByIdQuery getDomainEventById)
		=> new Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetDomainEventById(
			ConnectionProvider,
			getDomainEventById);

	public Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.IGetNextDomainEvents GetNextDomainEvents(
		Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetNextDomainEventsQuery getNextDomainEvents)
		=> new Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent.GetNextDomainEvents(
			ConnectionProvider,
			getNextDomainEvents);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.DomainEvents.Model.DomainEvent entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEvent.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.DomainEvents.Model.DomainEvent entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.DomainEvent.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEvent.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.DomainEvent.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.DomainEvents.Model.DomainEvent entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEvent.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.DomainEvent.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetDomainEventTableInfo();

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
