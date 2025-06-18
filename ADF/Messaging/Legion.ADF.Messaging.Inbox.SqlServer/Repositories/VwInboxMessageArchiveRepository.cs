using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.SqlServer.Model.Repositories;

public partial class VwInboxMessageArchiveRepository : Legion.ADF.Messaging.Inbox.SqlServer.InboxQueryRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>, Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageArchiveRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.SqlServer.IInboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>? AccessControlManager => _accessControlManager.Value;

	public VwInboxMessageArchiveRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.SqlServer.IInboxQueryDbContext>(scopeContext)).VwInboxMessageArchive;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.IGetAllVwInboxMessageArchivesByIdQueue GetAllVwInboxMessageArchivesByIdQueue(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery getAllVwInboxMessageArchivesByIdQueue)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueue(
			ConnectionProvider,
			getAllVwInboxMessageArchivesByIdQueue);

	public Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.IGetVwInboxMessageArchiveByIdMessage GetVwInboxMessageArchiveByIdMessage(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetVwInboxMessageArchiveByIdMessageQuery getVwInboxMessageArchiveByIdMessage)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetVwInboxMessageArchiveByIdMessage(
			ConnectionProvider,
			getVwInboxMessageArchiveByIdMessage);
}
