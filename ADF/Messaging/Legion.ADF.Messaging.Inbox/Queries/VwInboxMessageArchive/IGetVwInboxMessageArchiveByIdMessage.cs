namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive;

public partial interface IGetVwInboxMessageArchiveByIdMessage
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive? ToResult(
		Legion.IScopeContext scopeContext);
}
