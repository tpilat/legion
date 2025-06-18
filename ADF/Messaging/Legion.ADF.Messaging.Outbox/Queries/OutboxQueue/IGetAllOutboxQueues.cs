namespace Legion.ADF.Messaging.Outbox.Queries.OutboxQueue;

public partial interface IGetAllOutboxQueues
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.OutboxQueue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.OutboxQueue>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.OutboxQueue? ToResult(
		Legion.IScopeContext scopeContext);
}
