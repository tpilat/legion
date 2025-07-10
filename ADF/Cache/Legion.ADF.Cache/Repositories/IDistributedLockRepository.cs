namespace Legion.ADF.Cache.Model.Repositories;

public partial interface IDistributedLockRepository : Legion.ADF.Cache.ICacheRepository<Legion.ADF.Cache.Model.DistributedLock>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Cache.Model.DistributedLock>? AccessControlManager { get; }

	Legion.ADF.Cache.Queries.DistributedLock.IGetDistributedLockByKeyHash GetDistributedLockByKeyHash(
		Legion.ADF.Cache.Queries.DistributedLock.GetDistributedLockByKeyHashQuery getDistributedLockByKeyHash);
}
