namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage;

public partial interface IGetAllOutboxQueues
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
