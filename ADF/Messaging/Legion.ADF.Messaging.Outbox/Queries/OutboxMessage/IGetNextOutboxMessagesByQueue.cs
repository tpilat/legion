namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessage;

public partial interface IGetNextOutboxMessagesByQueue
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.OutboxMessage>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.OutboxMessage> ToResult(
		Legion.IScopeContext scopeContext);

	Task<Dictionary<Guid, DateTime>> ToOutboxMessageIds(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Dictionary<Guid, DateTime> ToOutboxMessageIds(
		Legion.IScopeContext scopeContext);
}
