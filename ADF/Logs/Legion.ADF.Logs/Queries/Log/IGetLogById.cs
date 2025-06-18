namespace Legion.ADF.Logs.Queries.Log;

public partial interface IGetLogById
{
	IQueryable<Legion.ADF.Logs.Model.Log> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Logs.Model.Log?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);
}
