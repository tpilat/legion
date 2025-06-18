namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEventProcessingLog;

public partial interface IGetAllDomainEventProcessingLogsByIdDomainEvent
{
	IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> ToResult(
		Legion.IScopeContext scopeContext);
}
