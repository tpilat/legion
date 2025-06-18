namespace Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType;

public partial interface IGetAllVwBlockedInboxMessageTypes
{
	IQueryable<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToNamespacesAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<string> ToNamespaces(
		Legion.IScopeContext scopeContext);
}
