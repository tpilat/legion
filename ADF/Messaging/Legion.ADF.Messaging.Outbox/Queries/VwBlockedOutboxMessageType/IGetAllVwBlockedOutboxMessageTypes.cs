namespace Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType;

public partial interface IGetAllVwBlockedOutboxMessageTypes
{
	IQueryable<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToNamespacesAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<string> ToNamespaces(
		Legion.IScopeContext scopeContext);
}
