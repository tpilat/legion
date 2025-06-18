namespace Legion.ADF.Cache.Model.Repositories;

public partial interface IVwReloadableCacheKeyRepository : Legion.ADF.Cache.ICacheQueryRepository<Legion.ADF.Cache.Model.VwReloadableCacheKey>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.VwReloadableCacheKey>? AccessControlManager { get; }

}
