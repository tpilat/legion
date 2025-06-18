namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage;

public partial interface IGetAllVwOutboxMessagesByIdQueue
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
