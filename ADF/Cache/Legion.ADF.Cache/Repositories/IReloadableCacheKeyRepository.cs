namespace Legion.ADF.Cache.Model.Repositories;

public partial interface IReloadableCacheKeyRepository : Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.ReloadableCacheKey>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.ReloadableCacheKey>? AccessControlManager { get; }

	Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetAllReloadableCacheKeys GetAllReloadableCacheKeys(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetAllReloadableCacheKeysQuery getAllReloadableCacheKeys);

	Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetAllReloadableCacheKeysByReloadAt GetAllReloadableCacheKeysByReloadAt(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetAllReloadableCacheKeysByReloadAtQuery getAllReloadableCacheKeysByReloadAt);

	Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetReloadableCacheKeyByKey GetReloadableCacheKeyByKey(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByKeyQuery getReloadableCacheKeyByKey);

	Legion.ADF.Cache.Queries.ReloadableCacheKey.IGetReloadableCacheKeyByTags GetReloadableCacheKeyByTags(
		Legion.ADF.Cache.Queries.ReloadableCacheKey.GetReloadableCacheKeyByTagsQuery getReloadableCacheKeyByTags);
}
