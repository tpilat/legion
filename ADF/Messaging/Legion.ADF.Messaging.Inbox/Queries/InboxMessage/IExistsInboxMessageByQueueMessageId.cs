namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessage;

public partial interface IExistsInboxMessageByQueueMessageId
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<bool> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdInboxMessageAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdInboxMessage(
		Legion.IScopeContext scopeContext);
}
