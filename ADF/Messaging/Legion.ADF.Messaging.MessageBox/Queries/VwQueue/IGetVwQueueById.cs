namespace Legion.ADF.Messaging.MessageBox.Queries.VwQueue;

public partial interface IGetVwQueueById
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwQueue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Messaging.MessageBox.Model.VwQueue?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.VwQueue? ToResult(
		Legion.IScopeContext scopeContext);
}
