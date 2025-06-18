namespace Legion.ADF.Messaging.Inbox.Queries.InboxQueue;

public partial interface IGetAllInboxQueues
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxQueue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.InboxQueue>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.InboxQueue? ToResult(
		Legion.IScopeContext scopeContext);
}
