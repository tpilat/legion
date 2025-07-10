namespace Legion.ADF.Cache.Queries.CacheData;

public partial interface IGetCacheDataByKeyHash
{
	IQueryable<Legion.ADF.Cache.Model.CacheData> GetQuery(
		Legion.IScopeContext scopeContext);

	Task<Legion.ADF.Cache.Model.CacheData?> ToResultAsync(
		Legion.IScopeContext scopeContext,
		CancellationToken cancellationToken = default);

	Legion.ADF.Cache.Model.CacheData? ToResult(
		Legion.IScopeContext scopeContext);
}
