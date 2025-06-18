using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.SqlServer.Model.Repositories;

public partial class VwBlockedOutboxMessageTypeRepository : Legion.ADF.Messaging.Outbox.SqlServer.OutboxQueryRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>, Legion.ADF.Messaging.Outbox.Model.Repositories.IVwBlockedOutboxMessageTypeRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>? AccessControlManager => _accessControlManager.Value;

	public VwBlockedOutboxMessageTypeRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.SqlServer.IOutboxQueryDbContext>(scopeContext)).VwBlockedOutboxMessageType;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType.IGetAllVwBlockedOutboxMessageTypes GetAllVwBlockedOutboxMessageTypes(
		Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType.GetAllVwBlockedOutboxMessageTypesQuery getAllVwBlockedOutboxMessageTypes)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType.GetAllVwBlockedOutboxMessageTypes(
			ConnectionProvider,
			getAllVwBlockedOutboxMessageTypes);
}
