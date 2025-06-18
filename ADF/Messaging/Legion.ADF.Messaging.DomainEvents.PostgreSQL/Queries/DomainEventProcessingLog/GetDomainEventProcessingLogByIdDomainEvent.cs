using Legion.ADF.Messaging.DomainEvents.PostgreSQL;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog;

public class GetAllDomainEventProcessingLogsByIdDomainEvent :
	QueryDefinition<
		IDomainEventsDbContext,
		DomainEvents.Model.DomainEventProcessingLog,
		List<DomainEvents.Model.DomainEventProcessingLog>,
		GetAllDomainEventProcessingLogsByIdDomainEventQuery>,
		IGetAllDomainEventProcessingLogsByIdDomainEvent
{
	public GetAllDomainEventProcessingLogsByIdDomainEvent(
		IEFConnectionProvider connectionProvider,
		GetAllDomainEventProcessingLogsByIdDomainEventQuery getAllDomainEventProcessingLogsByIdDomainEventQuery)
		: base(connectionProvider, getAllDomainEventProcessingLogsByIdDomainEventQuery)
	{
	}

	protected override IQueryable<DomainEvents.Model.DomainEventProcessingLog> GetDefaultQuery(IScopeContext scopeContext)
	{
		var context = GetContext(scopeContext);
		return context.DomainEventProcessingLog;
	}

	public override IQueryable<DomainEvents.Model.DomainEventProcessingLog> GetQuery(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return ApplyIncludesThenWhere<IMessagingAccessControlManager>(
			scopeContext,
			QueryRequest.QueryableBuilder,
			QueryRequest.CheckReadPermissions,
			QueryRequest.AsNoTracking,
			de => de.IdDomainEvent == QueryRequest.IdDomainEvent);
	}

	public override async Task<List<DomainEvents.Model.DomainEventProcessingLog>> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.ToListAsync(cancellationToken);
	}

	public List<DomainEvents.Model.DomainEventProcessingLog> ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).ToList();
	}
}
