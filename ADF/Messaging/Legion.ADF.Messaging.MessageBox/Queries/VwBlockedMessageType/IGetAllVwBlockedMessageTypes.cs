namespace Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType;

public partial interface IGetAllVwBlockedMessageTypes
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> ToResult(
		Legion.IScopeContext scopeContext);

	Task<List<string>> ToNamespacesAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<string> ToNamespaces(
		Legion.IScopeContext scopeContext);
}
