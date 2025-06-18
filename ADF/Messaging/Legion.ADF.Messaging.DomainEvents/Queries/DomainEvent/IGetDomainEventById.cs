namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEvent;

public partial interface IGetDomainEventById
{
	IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.DomainEvents.Model.DomainEvent? ToResult(
		Legion.IScopeContext scopeContext);
}
