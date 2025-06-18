namespace Legion.ADF.Messaging.MessageBox.Queries.MessageType;

public partial interface IGetMessageTypeByNamespace
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.MessageType> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.MessageType?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.MessageType? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdMessageTypeAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdMessageType(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
