namespace Legion.ADF.Messaging.Inbox.Queries.InboxQueue;

public partial interface IGetInboxQueueByName
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxQueue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.InboxQueue?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.InboxQueue? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdInboxQueueAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdInboxQueue(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
