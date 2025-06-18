namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent;

public partial interface IExistsDomainEventByIdDomainEvent
{
	IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdDomainEventAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdDomainEvent(
		Legion.IScopeContext scopeContext);
}
