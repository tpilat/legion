using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.PostgreSQL.Model.Repositories;

public partial class VwBlockedInboxMessageTypeRepository : Legion.ADF.Messaging.Inbox.PostgreSQL.InboxQueryRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType>, Legion.ADF.Messaging.Inbox.Model.Repositories.IVwBlockedInboxMessageTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType>? AccessControlManager => _accessControlManager.Value;

	public VwBlockedInboxMessageTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxQueryDbContext>(scopeContext)).VwBlockedInboxMessageType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType.IGetAllVwBlockedInboxMessageTypes GetAllVwBlockedInboxMessageTypes(
		Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType.GetAllVwBlockedInboxMessageTypesQuery getAllVwBlockedInboxMessageTypes)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType.GetAllVwBlockedInboxMessageTypes(
			ConnectionProvider,
			getAllVwBlockedInboxMessageTypes);
}
