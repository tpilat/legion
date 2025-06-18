namespace Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType;

public partial interface IGetBlockedInboxMessageTypesByNamespaces
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToNamespacesAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<string> ToNamespaces(
		Legion.IScopeContext scopeContext);
}
