namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog;

public partial interface IGetVwInboxMessageProcessingLogsByIdMessage
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> ToResult(
		Legion.IScopeContext scopeContext);
}
