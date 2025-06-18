using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL.Model.Repositories;

public partial class VwOutboxMessageContentRepository : Legion.ADF.Messaging.Outbox.PostgreSQL.OutboxQueryRepositoryBase, Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent>, Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageContentRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent>?> _accessControlManager;

	private Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent>? AccessControlManager => _accessControlManager.Value;

	public VwOutboxMessageContentRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent>>());
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxQueryDbContext>(scopeContext)).VwOutboxMessageContent;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	
	public Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent.IGetVwOutboxMessageContentById GetVwOutboxMessageContentById(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent.GetVwOutboxMessageContentByIdQuery getVwOutboxMessageContentByIdMessage)
		=> new Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent.GetVwOutboxMessageContentById(
			ConnectionProvider,
			getVwOutboxMessageContentByIdMessage);
}
