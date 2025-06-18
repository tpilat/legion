using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL.Model.Repositories;

public partial class QueuedMessageRepository : Legion.ADF.Messaging.MessageBox.PostgreSQL.MessageBoxRepositoryBase, Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>, Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueuedMessageRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>?> _accessControlManager;

	private Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>? AccessControlManager => _accessControlManager.Value;

	public QueuedMessageRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>>());
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext>(scopeContext)).QueuedMessage;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.IGetNextQueuedMessagesByQueue GetNextQueuedMessagesByQueue(
		Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetNextQueuedMessagesByQueueQuery getNextQueuedMessagesByQueue)
		=> new Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetNextQueuedMessagesByQueue(
			ConnectionProvider,
			getNextQueuedMessagesByQueue);

	public Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.IGetQueuedMessageById GetQueuedMessageById(
		Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessageByIdQuery getQueuedMessageByIdQuery)
		=> new Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessageById(
			ConnectionProvider,
			getQueuedMessageByIdQuery);

	public Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.IGetQueuedMessagesByIdMessage GetQueuedMessagesByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessagesByIdMessageQuery getQueuedMessagesByIdMessage)
		=> new Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessagesByIdMessage(
			ConnectionProvider,
			getQueuedMessagesByIdMessage);

	public void Add(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.QueuedMessage entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.QueuedMessage.Add(entity);
	}

	public async Task AddAsync(
		IScopeContext scopeContext,
		Legion.ADF.Messaging.MessageBox.Model.QueuedMessage entity,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.QueuedMessage.AddAsync(entity, cancellationToken);
	}

	public void AddRange(IScopeContext scopeContext, IEnumerable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.QueuedMessage.AddRange(entities);
	}

	public async Task AddRangeAsync(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> entities,
		CancellationToken cancellationToken = default)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		await dbContext.QueuedMessage.AddRangeAsync(entities, cancellationToken);
	}

	public void Remove(IScopeContext scopeContext, Legion.ADF.Messaging.MessageBox.Model.QueuedMessage entity)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.QueuedMessage.Remove(entity);
	}

	public void RemoveRange(
		IScopeContext scopeContext,
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> entities)
	{
		var dbContext = GetContext(scopeContext);
		Throw.IfNull(dbContext);

		dbContext.QueuedMessage.RemoveRange(entities);
	}


	public ulong BulkInsert(
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> entities,
		bool allowCreateNewDbConnection = false)
	{
		var tableInfo = TableInfoProvider.GetQueuedMessageTableInfo();
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
		IEnumerable<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> entities,
		bool allowCreateNewDbConnection = false,
		CancellationToken cancellationToken = default)
	{
		var tableInfo = TableInfoProvider.GetQueuedMessageTableInfo();
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
