namespace Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType;

public partial interface IGetBlockedOutboxMessageTypesByNamespaces
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToNamespacesAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<string> ToNamespaces(
		Legion.IScopeContext scopeContext);
}
