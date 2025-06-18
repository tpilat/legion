namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive;

public partial interface IGetVwOutboxMessageArchiveByIdMessage
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive? ToResult(
		Legion.IScopeContext scopeContext);
}
