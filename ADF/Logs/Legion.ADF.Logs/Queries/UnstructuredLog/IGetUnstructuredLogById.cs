namespace Legion.ADF.Logs.Queries.UnstructuredLog;

public partial interface IGetUnstructuredLogById
{
	IQueryable<Legion.ADF.Logs.Model.UnstructuredLog> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Logs.Model.UnstructuredLog?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
