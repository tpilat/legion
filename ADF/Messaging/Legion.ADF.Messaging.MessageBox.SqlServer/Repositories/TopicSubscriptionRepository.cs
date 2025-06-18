using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.SqlServer.Model.Repositories;

public partial class TopicSubscriptionRepository : Legion.ADF.Messaging.MessageBox.SqlServer.MessageBoxRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>, Legion.ADF.Messaging.MessageBox.Model.Repositories.ITopicSubscriptionRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>? AccessControlManager => _accessControlManager.Value;

	public TopicSubscriptionRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext>(scopeContext)).TopicSubscription;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetAllTopicSubscriptions GetAllTopicSubscriptions(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsQuery getAllTopicSubscriptions)
		=> new Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptions(
			ConnectionProvider,
			getAllTopicSubscriptions);

	public Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetAllTopicSubscriptionsByTopic GetAllTopicSubscriptionsByTopic(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicQuery getAllTopicSubscriptionsByTopic)
		=> new Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopic(
			ConnectionProvider,
			getAllTopicSubscriptionsByTopic);

	public Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetAllTopicSubscriptionsByTopicAndEvents GetAllTopicSubscriptionsByTopicAndEvents(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicAndEventsQuery getAllTopicSubscriptionsByTopicAndEvents)
		=> new Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicAndEvents(
			ConnectionProvider,
			getAllTopicSubscriptionsByTopicAndEvents);

	public Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetTopicSubscriptionByTopicAndName GetTopicSubscriptionByTopicAndName(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetTopicSubscriptionByTopicAndNameQuery getTopicSubscriptionByTopicAndName)
		=> new Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetTopicSubscriptionByTopicAndName(
			ConnectionProvider,
			getTopicSubscriptionByTopicAndName);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.TopicSubscription entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.TopicSubscription.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.MessageBox.Model.TopicSubscription entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.TopicSubscription.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.TopicSubscription.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.TopicSubscription.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.TopicSubscription entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.TopicSubscription.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.TopicSubscription.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetTopicSubscriptionTableInfo();

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
