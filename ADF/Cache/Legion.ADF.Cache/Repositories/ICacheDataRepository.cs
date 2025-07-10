namespace Legion.ADF.Cache.Model.Repositories;

public partial interface ICacheDataRepository : Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.CacheData>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.CacheData>? AccessControlManager { get; }

	Legion.ADF.Cache.Queries.CacheData.IGetCacheDataByKeyHash GetCacheDataByKeyHash(
		Legion.ADF.Cache.Queries.CacheData.GetCacheDataByKeyHashQuery getCacheDataByKeyHash);
}
