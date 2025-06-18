namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage;

public partial interface IGetAllVwInboxMessagesByIdQueue
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
