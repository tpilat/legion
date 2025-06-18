using Legion.ADF.Messaging.DomainEvents.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType;

public class GetAllBlockedDomainEventTypes :
	QueryDefinition<
		IDomainEventsDbContext,
		DomainEvents.Model.BlockedDomainEventType,
		List<DomainEvents.Model.BlockedDomainEventType>,
		GetAllBlockedDomainEventTypesQuery>,
		IGetAllBlockedDomainEventTypes
{
	public GetAllBlockedDomainEventTypes(
		IEFConnectionProvider connectionProvider,
		GetAllBlockedDomainEventTypesQuery getAllBlockedDomainEventTypes)
		: base(connectionProvider, getAllBlockedDomainEventTypes)
	{
	}

	protected override IQueryable<DomainEvents.Model.BlockedDomainEventType> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.BlockedDomainEventType;
	}

	public override IQueryable<DomainEvents.Model.BlockedDomainEventType> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			null);
	}

	public override async Task<List<DomainEvents.Model.BlockedDomainEventType>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<DomainEvents.Model.BlockedDomainEventType> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<List<string>> ToNamespacesAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(bde => bde.Namespace)
			.ToListAsync(cancellationToken);
	}

	public List<string> ToNamespaces(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(bde => bde.Namespace)
			.ToList();
	}
}
