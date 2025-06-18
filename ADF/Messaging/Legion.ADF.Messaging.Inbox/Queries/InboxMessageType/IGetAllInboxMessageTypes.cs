namespace Legion.ADF.Messaging.Inbox.Queries.InboxMessageType;

public partial interface IGetAllInboxMessageTypes
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.InboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.InboxMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.InboxMessageType> ToResult(
		Legion.IScopeContext scopeContext);
}
