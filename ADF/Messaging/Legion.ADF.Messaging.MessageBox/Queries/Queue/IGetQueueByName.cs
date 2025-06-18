namespace Legion.ADF.Messaging.MessageBox.Queries.Queue;

public partial interface IGetQueueByName
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.Queue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.Queue?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.Queue? ToResult(
		Legion.IScopeContext scopeContext);

	Task<Guid?> GetIdQueueAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Guid? GetIdQueue(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
