using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.SqlServer.Model.Repositories;

public partial class VwOutboxMessageArchiveRepository : Legion.ADF.Messaging.Outbox.SqlServer.OutboxQueryRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>, Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageArchiveRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>? AccessControlManager => _accessControlManager.Value;

	public VwOutboxMessageArchiveRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext>(scopeContext)).VwOutboxMessageArchive;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.IGetAllVwOutboxMessageArchivesByIdQueue GetAllVwOutboxMessageArchivesByIdQueue(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery getAllVwOutboxMessageArchivesByIdQueue)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueue(
			ConnectionProvider,
			getAllVwOutboxMessageArchivesByIdQueue);

	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.IGetVwOutboxMessageArchiveByIdMessage GetVwOutboxMessageArchiveByIdMessage(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetVwOutboxMessageArchiveByIdMessageQuery getVwOutboxMessageArchiveByIdMessage)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetVwOutboxMessageArchiveByIdMessage(
			ConnectionProvider,
			getVwOutboxMessageArchiveByIdMessage);
}
