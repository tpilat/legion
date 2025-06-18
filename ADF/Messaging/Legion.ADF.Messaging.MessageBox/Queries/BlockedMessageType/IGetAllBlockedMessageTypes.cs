namespace Legion.ADF.Messaging.MessageBox.Queries.BlockedMessageType;

public partial interface IGetAllBlockedMessageTypes
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToNamespacesAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<string> ToNamespaces(
		Legion.IScopeContext scopeContext);
}
