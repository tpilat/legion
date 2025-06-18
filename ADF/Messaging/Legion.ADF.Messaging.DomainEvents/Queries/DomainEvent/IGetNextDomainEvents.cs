namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent;

public partial interface IGetNextDomainEvents
{
	IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<Guid>> ToDomainEventIds(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Guid> ToDomainEventIds(
		Legion.IScopeContext scopeContext);
}
