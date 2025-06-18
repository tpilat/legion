namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessage;

public partial interface IGetAllVwMessagesByIdQueue
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessage> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.VwMessage>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.VwMessage> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
