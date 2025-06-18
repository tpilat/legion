namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxQueue;

public partial interface IGetVwInboxQueueById
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxQueue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.VwInboxQueue?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.VwInboxQueue? ToResult(
		Legion.IScopeContext scopeContext);
}
