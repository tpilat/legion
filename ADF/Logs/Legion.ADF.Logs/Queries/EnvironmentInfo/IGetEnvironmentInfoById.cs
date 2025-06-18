namespace Legion.ADF.Logs.Queries.EnvironmentInfo;

public partial interface IGetEnvironmentInfoById
{
	IQueryable<Legion.ADF.Logs.Model.EnvironmentInfo> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Logs.Model.EnvironmentInfo?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Logs.Model.EnvironmentInfo? ToResult(
		Legion.IScopeContext scopeContext);

	Task<bool> ExistsAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	bool Exists(
		Legion.IScopeContext scopeContext);
}
