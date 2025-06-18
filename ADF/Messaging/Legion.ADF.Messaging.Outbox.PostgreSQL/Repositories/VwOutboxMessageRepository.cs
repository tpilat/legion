using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories;

public partial class VwOutboxMessageRepository : Legion.ADF.Messaging.Outbox.PostgreSQL.OutboxQueryRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage>, Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage>? AccessControlManager => _accessControlManager.Value;

	public VwOutboxMessageRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext>(scopeContext)).VwOutboxMessage;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.IGetAllVwOutboxMessagesByIdQueue GetAllVwOutboxMessagesByIdQueue(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueueQuery getAllVwOutboxMessagesByIdQueue)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueue(
			ConnectionProvider,
			getAllVwOutboxMessagesByIdQueue);

	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.IGetVwOutboxMessageByIdMessage GetVwOutboxMessageByIdMessage(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.GetVwOutboxMessageByIdMessageQuery getVwOutboxMessageByIdMessage)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.GetVwOutboxMessageByIdMessage(
			ConnectionProvider,
			getVwOutboxMessageByIdMessage);
}
