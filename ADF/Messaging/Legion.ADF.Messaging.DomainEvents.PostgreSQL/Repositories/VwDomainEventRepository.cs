using Legion;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Legion.Exceptions;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL.Model.Repositories;

public partial class VwDomainEventRepository : Legion.ADF.Messaging.DomainEvents.PostgreSQL.DomainEventsQueryRepositoryBase, Legion.ADF.Messaging.DomainEvents.IDomainEventsQueryRepository<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent>, Legion.ADF.Messaging.DomainEvents.Model.Repositories.IVwDomainEventRepository
{
	private readonly Lazy<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent>?> _accessControlManager;

	private Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsQueryDbContext? _context;

	public Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent>? AccessControlManager => _accessControlManager.Value;

	public VwDomainEventRepository(IEFConnectionProvider connectionProvider)
		: base(connectionProvider)
	{
		_accessControlManager = new(() => connectionProvider.ServiceProvider.GetService<Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent>>());
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent> AsQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext, false);

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent> AsQueryable(IScopeContext scopeContext, bool checkReadPermissions)
	{
		var queryable = (_context ??= ConnectionProvider.GetOrCreateDbContext<Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsQueryDbContext>(scopeContext)).VwDomainEvent;

		if (checkReadPermissions)
			AccessControlManager?.SetAuthorizationQuery(scopeContext, queryable);

		return queryable;
	}

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent> AsReadOnlyQueryable(IScopeContext scopeContext)
		=> AsQueryable(scopeContext).AsNoTracking();

	public IQueryable<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent> AsReadOnlyQueryable(IScopeContext scopeContext, bool checkReadPermissions)
		=> AsQueryable(scopeContext, checkReadPermissions).AsNoTracking();
	}
