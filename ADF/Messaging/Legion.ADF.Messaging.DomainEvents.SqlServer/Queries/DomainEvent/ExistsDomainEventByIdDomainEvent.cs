using Legion.ADF.Messaging.DomainEvents.SqlServer;
using Legion.EntityFrameworkCore;
using Legion.EntityFrameworkCore.Queries;
using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent;

public class ExistsDomainEventByIdDomainEvent :
	QueryDefinition<
		IDomainEventsDbContext,
		DomainEvents.Model.DomainEvent,
		bool,
		ExistsDomainEventByIdDomainEventQuery>,
		IExistsDomainEventByIdDomainEvent
{
	public ExistsDomainEventByIdDomainEvent(
		IEFConnectionProvider connectionProvider,
		ExistsDomainEventByIdDomainEventQuery existsDomainEventByIdDomainEvent)
		: base(connectionProvider, existsDomainEventByIdDomainEvent)
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
			de => de.IdDomainEvent == QueryRequest.IdDomainEvent);
	}

	public override async Task<bool> ToResultAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.AnyAsync(cancellationToken);
	}

	public bool ToResult(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext).Any();
	}

	public async Task<Guid?> GetIdDomainEventAsync(
		IScopeContext scopeContext,
		CancellationToken cancellationToken = default)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return await GetQuery(scopeContext)
			.Select(iq => iq.IdDomainEvent)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public Guid? GetIdDomainEvent(IScopeContext scopeContext)
	{
		scopeContext = ScopeContext.Create(scopeContext);

		return GetQuery(scopeContext)
			.Select(iq => iq.IdDomainEvent)
			.FirstOrDefault();
	}
}
