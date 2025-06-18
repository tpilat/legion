namespace Legion.ADF.Messaging.MessageBox.Queries.Queue;

public partial interface IGetAllQueuesByEvents
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.Queue> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.Queue>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Messaging.MessageBox.Model.Queue? ToResult(
		Legion.IScopeContext scopeContext);
}
