namespace Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog;

public partial interface IGetVwOutboxMessageProcessingLogsByIdMessage
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> ToResult(
		Legion.IScopeContext scopeContext);
}
