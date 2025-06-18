namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessage;

public partial interface IGetInboxMessageById
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.Inbox.Model.InboxMessage?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.Inbox.Model.InboxMessage? ToResult(
		Legion.IScopeContext scopeContext);
}
