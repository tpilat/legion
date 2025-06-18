using Legion.ADF.Messaging.DomainEvents.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent;

public class GetDomainEventById :
	QueryDefinition<
		IDomainEventsDbContext,
		DomainEvents.Model.DomainEvent,
		DomainEvents.Model.DomainEvent?,
		GetDomainEventByIdQuery>,
		IGetDomainEventById
{
	public GetDomainEventById(
		IEFConnectionProvider connectionProvider,
		GetDomainEventByIdQuery getDomainEventById)
		: base(connectionProvider, getDomainEventById)
	{
	}

	protected override IQueryable<DomainEvents.Model.DomainEvent> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return QueryRequest.IncludeContent
			? context.DomainEvent.Include(de => de.Content)
			: context.DomainEvent;
	}

	public override IQueryable<DomainEvents.Model.DomainEvent> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			de => de.IdDomainEvent == QueryRequest.IdDomainEvent);
	}

	public override async Task<DomainEvents.Model.DomainEvent?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public DomainEvents.Model.DomainEvent? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
