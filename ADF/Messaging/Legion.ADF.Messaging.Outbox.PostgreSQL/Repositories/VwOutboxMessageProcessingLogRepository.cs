using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories;

public partial class VwOutboxMessageProcessingLogRepository : Legion.ADF.Messaging.Outbox.PostgreSQL.OutboxQueryRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog>, Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageProcessingLogRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog>? AccessControlManager => _accessControlManager.Value;

	public VwOutboxMessageProcessingLogRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext>(scopeContext)).VwOutboxMessageProcessingLog;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog.IGetVwOutboxMessageProcessingLogsByIdMessage GetVwOutboxMessageProcessingLogsByIdMessage(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery getVwOutboxMessageProcessingLogByIdMessage)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessage(
			ConnectionProvider,
			getVwOutboxMessageProcessingLogByIdMessage);
}
