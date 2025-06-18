namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessage;

public partial interface IGetNextInboxMessagesByQueue
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.InboxMessage>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.InboxMessage> ToResult(
		Legion.IScopeContext scopeContext);

	Task<Dictionary<Guid, DateTime>> ToInboxMessageIds(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Dictionary<Guid, DateTime> ToInboxMessageIds(
		Legion.IScopeContext scopeContext);
}
