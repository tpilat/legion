using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.SqlServer.Model.Repositories;

public partial class SubscribedMessageRepository : Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>, Legion.ADF.Messaging.MessageBox.Model.Repositories.ISubscribedMessageRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>? AccessControlManager => _accessControlManager.Value;

	public SubscribedMessageRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext>(scopeContext)).SubscribedMessage;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.IGetNextSubscribedMessagesBySubscription GetNextSubscribedMessagesBySubscription(
		Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetNextSubscribedMessagesBySubscriptionQuery getNextSubscribedMessagesBySubscription)
		=> new Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetNextSubscribedMessagesBySubscription(
			ConnectionProvider,
			getNextSubscribedMessagesBySubscription);

	public Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.IGetSubscribedMessageById GetSubscribedMessageById(
		Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessageByIdQuery getSubscribedMessageById)
		=> new Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessageById(
			ConnectionProvider,
			getSubscribedMessageById);

	public Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.IGetSubscribedMessagesByIdMessage GetSubscribedMessagesByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessagesByIdMessageQuery getSubscribedMessagesByIdMessage)
		=> new Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessagesByIdMessage(
			ConnectionProvider,
			getSubscribedMessagesByIdMessage);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.SubscribedMessage.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.SubscribedMessage.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.SubscribedMessage.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.SubscribedMessage.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.SubscribedMessage.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.SubscribedMessage.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetSubscribedMessageTableInfo();

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
