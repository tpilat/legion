namespace Legion.ADF.Messaging.DomainEvents.Queries.BlockedDomainEventType;

public partial interface IGetAllBlockedDomainEventTypes
{
	IQueryable<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToNamespacesAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<string> ToNamespaces(
		Legion.IScopeContext scopeContext);
}
