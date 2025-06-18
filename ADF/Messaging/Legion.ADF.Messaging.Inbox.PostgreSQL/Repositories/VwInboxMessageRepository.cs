using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories;

public partial class VwInboxMessageRepository : Legion.ADF.Messaging.Inbox.PostgreSQL.InboxQueryRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage>, Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage>? AccessControlManager => _accessControlManager.Value;

	public VwInboxMessageRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext>(scopeContext)).VwInboxMessage;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.IGetAllVwInboxMessagesByIdQueue GetAllVwInboxMessagesByIdQueue(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueueQuery getAllVwInboxMessagesByIdQueue)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueue(
			ConnectionProvider,
			getAllVwInboxMessagesByIdQueue);

	public Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.IGetVwInboxMessageByIdMessage GetVwInboxMessageByIdMessage(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetVwInboxMessageByIdMessageQuery getVwInboxMessageByIdMessage)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetVwInboxMessageByIdMessage(
			ConnectionProvider,
			getVwInboxMessageByIdMessage);
}
