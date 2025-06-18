namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive;

public partial interface IGetAllVwInboxMessageArchivesByIdQueue
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
