namespace Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent;

public partial interface IGetVwInboxMessageContentById
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent? ToResult(
		Legion.IScopeContext scopeContext);
}
