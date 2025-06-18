namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive;

public partial interface IGetAllVwOutboxMessageArchivesByIdQueue
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
