using Legion.ADF.Messaging.DomainEvents.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEventContent;

public class GetDomainEventContentById :
	QueryDefinition<
		IDomainEventsDbContext,
		DomainEvents.Model.DomainEventContent,
		DomainEvents.Model.DomainEventContent?,
		GetDomainEventContentByIdQuery>,
		IGetDomainEventContentById
{
	public GetDomainEventContentById(
		IEFConnectionProvider connectionProvider,
		GetDomainEventContentByIdQuery getDomainEventContentById)
		: base(connectionProvider, getDomainEventContentById)
	{
	}

	protected override IQueryable<DomainEvents.Model.DomainEventContent> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.DomainEventContent;
	}

	public override IQueryable<DomainEvents.Model.DomainEventContent> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			dec => dec.IdDomainEventContent == QueryRequest.IdDomainEvent);
	}

	public override async Task<DomainEvents.Model.DomainEventContent?> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public DomainEvents.Model.DomainEventContent? ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).FirstOrDefault();
	}
}
