namespace Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage;

public partial interface IGetAllQueues
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
