using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories;

public partial class VwInboxMessageContentRepository : Legion.ADF.Messaging.Inbox.PostgreSQL.InboxQueryRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent>, Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageContentRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent>? AccessControlManager => _accessControlManager.Value;

	public VwInboxMessageContentRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext>(scopeContext)).VwInboxMessageContent;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent.IGetVwInboxMessageContentById GetVwInboxMessageContentById(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent.GetVwInboxMessageContentByIdQuery getVwInboxMessageContentByIdMessage)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent.GetVwInboxMessageContentById(
			ConnectionProvider,
			getVwInboxMessageContentByIdMessage);
}
