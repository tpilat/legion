namespace Legion.ADF.Messaging.Outbox.Queries.OutboxQueue;

public partial interface IGetOutboxQueueByName
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxQueue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.OutboxQueue?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.OutboxQueue? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdOutboxQueueAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdOutboxQueue(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
