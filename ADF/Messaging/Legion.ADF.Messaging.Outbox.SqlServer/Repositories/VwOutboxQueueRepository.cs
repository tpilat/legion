using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.SqlServer.Model.Repositories;

public partial class VwOutboxQueueRepository : Legion.ADF.Messaging.Outbox.SqlServer.OutboxQueryRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue>, Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue>? AccessControlManager => _accessControlManager.Value;

	public VwOutboxQueueRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext>(scopeContext)).VwOutboxQueue;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue.IGetVwOutboxQueueById GetVwOutboxQueueById(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue.GetVwOutboxQueueByIdQuery getVwOutboxQueueById)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue.GetVwOutboxQueueById(
			ConnectionProvider,
			getVwOutboxQueueById);
}
