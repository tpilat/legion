namespace Legion.ADF.Messaging.Outbox.Queries.OutboxMessage;

public partial interface IExistsOutboxMessageByQueueMessageId
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdOutboxMessageAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdOutboxMessage(
		Legion.IScopeContext scopeContext);
}
