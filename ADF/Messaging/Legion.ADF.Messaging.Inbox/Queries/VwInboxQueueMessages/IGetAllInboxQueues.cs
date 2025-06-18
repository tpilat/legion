namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage;

public partial interface IGetAllInboxQueues
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
