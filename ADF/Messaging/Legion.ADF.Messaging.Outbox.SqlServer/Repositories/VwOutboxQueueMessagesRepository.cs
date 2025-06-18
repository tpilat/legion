using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.SqlServer.Model.Repositories;

public partial class VwOutboxQueueMessagesRepository : Legion.ADF.Messaging.Outbox.SqlServer.OutboxQueryRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages>, Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueMessagesRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages>? AccessControlManager => _accessControlManager.Value;

	public VwOutboxQueueMessagesRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext>(scopeContext)).VwOutboxQueueMessages;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage.IGetAllOutboxQueues GetAllOutboxQueues(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage.GetAllOutboxQueuesQuery getAllOutboxQueues)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage.GetAllOutboxQueues(
			ConnectionProvider,
			getAllOutboxQueues);
}
