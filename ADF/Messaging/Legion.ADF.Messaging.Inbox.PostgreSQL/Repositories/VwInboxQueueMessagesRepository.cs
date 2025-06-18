using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories;

public partial class VwInboxQueueMessagesRepository : Legion.ADF.Messaging.Inbox.PostgreSQL.InboxQueryRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages>, Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxQueueMessagesRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages>? AccessControlManager => _accessControlManager.Value;

	public VwInboxQueueMessagesRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext>(scopeContext)).VwInboxQueueMessages;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage.IGetAllInboxQueues GetAllInboxQueues(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage.GetAllInboxQueuesQuery getAllInboxQueues)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage.GetAllInboxQueues(
			ConnectionProvider,
			getAllInboxQueues);
}
