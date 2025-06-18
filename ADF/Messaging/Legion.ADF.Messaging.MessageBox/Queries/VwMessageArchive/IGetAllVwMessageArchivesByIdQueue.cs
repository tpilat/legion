namespace Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive;

public partial interface IGetAllVwMessageArchivesByIdQueue
{
	IQueryable<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<List<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive>> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	List<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> ToResult(
		Legion.IScopeContext scopeContext);

	Task<long> TotalCountAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	long TotalCount(
		Legion.IScopeContext scopeContext);
}
