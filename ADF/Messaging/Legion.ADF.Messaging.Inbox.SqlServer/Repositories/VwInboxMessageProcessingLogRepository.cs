using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.SqlServer.Model.Repositories;

public partial class VwInboxMessageProcessingLogRepository : Legion.ADF.Messaging.Inbox.SqlServer.InboxQueryRepositoryBase, Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog>, Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageProcessingLogRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog>?> _accessControlManager;

	private Legion.ADF.Messaging.Inbox.SqlServer.IInboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog>? AccessControlManager => _accessControlManager.Value;

	public VwInboxMessageProcessingLogRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog>>());
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Inbox.SqlServer.IInboxQueryDbContext>(scopeContext)).VwInboxMessageProcessingLog;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog.IGetVwInboxMessageProcessingLogsByIdMessage GetVwInboxMessageProcessingLogsByIdMessage(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery getVwInboxMessageProcessingLogByIdMessage)
		=> new Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessage(
			ConnectionProvider,
			getVwInboxMessageProcessingLogByIdMessage);
}
