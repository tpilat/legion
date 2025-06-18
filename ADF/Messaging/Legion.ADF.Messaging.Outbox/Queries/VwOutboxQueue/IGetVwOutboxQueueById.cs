namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue;

public partial interface IGetVwOutboxQueueById
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue? ToResult(
		Legion.IScopeContext scopeContext);
}
