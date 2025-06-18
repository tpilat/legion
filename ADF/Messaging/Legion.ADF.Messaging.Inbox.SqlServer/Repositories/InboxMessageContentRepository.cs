using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.SqlServer.Model.Repositories;

public partial class InboxMessageContentRepository : Legion.ADF.Messaging.Inbox.SqlServer.InboxRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent>, Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxMessageContentRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent>? AccessControlManager => _accessControlManager.Value;

	public InboxMessageContentRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext>(scopeContext)).InboxMessageContent;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.Inbox.Model.InboxMessageContent entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.InboxMessageContent.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.Inbox.Model.InboxMessageContent entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.InboxMessageContent.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.InboxMessageContent.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.InboxMessageContent.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.Inbox.Model.InboxMessageContent entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.InboxMessageContent.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.InboxMessageContent.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetInboxMessageContentTableInfo();

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
