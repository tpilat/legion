using Legion.ADF.Messaging.DomainEvents.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent;

public class GetNextDomainEvents :
	QueryDefinition<
		IDomainEventsDbContext,
		DomainEvents.Model.DomainEvent,
		List<DomainEvents.Model.DomainEvent>,
		GetNextDomainEventsQuery>,
		IGetNextDomainEvents
{
	public GetNextDomainEvents(
		IEFConnectionProvider connectionProvider,
		GetNextDomainEventsQuery getNextDomainEvents)
		: base(connectionProvider, getNextDomainEvents)
	{
	}

	protected override IQueryable<DomainEvents.Model.DomainEvent> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.DomainEvent;
	}

	public override IQueryable<DomainEvents.Model.DomainEvent> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			de =>
				!de.ProcessedUtc.HasValue
				&& !de.SuspendedUtc.HasValue
				&& de.NextProcessingUtc <= QueryRequest.NowUtc)
			.OrderBy(de => de.NextProcessingUtc)
			.Take(QueryRequest.BatchCount);
	}

	public override async Task<List<DomainEvents.Model.DomainEvent>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<DomainEvents.Model.DomainEvent> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}

	public async Task<List<Guid>> ToDomainEventIds(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(de => de.IdDomainEvent)
			.ToListAsync(cancellationToken);
	}

	public List<Guid> ToDomainEventIds(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(de => de.IdDomainEvent)
			.ToList();
	}
}
