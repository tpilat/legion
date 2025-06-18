namespace Legion.ADF.Messaging.DomainEvents.Queries.DomainEventContent;

public partial interface IGetDomainEventContentById
{
	IQueryable<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent? ToResult(
		Legion.IScopeContext scopeContext);
}
