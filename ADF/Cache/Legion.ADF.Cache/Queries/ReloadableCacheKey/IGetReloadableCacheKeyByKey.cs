namespace Legion.ADF.Cache.Queries.ReloadableCacheKey;

public partial interface IGetReloadableCacheKeyByKey
{
	IQueryable<Legion.ADF.Cache.Model.ReloadableCacheKey> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Cache.Model.ReloadableCacheKey?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Cache.Model.ReloadableCacheKey? ToResult(
		Legion.IScopeContext scopeContext);
}
