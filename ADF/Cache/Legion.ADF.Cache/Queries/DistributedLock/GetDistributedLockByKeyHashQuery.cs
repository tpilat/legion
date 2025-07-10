using Legion.MessageBus.Messages;

namespace Legion.ADF.Cache.Queries.DistributedLock;

public record GetDistributedLockByKeyHashQuery(
	string KeyHash,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Cache.Model.DistributedLock>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.DistributedLock>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Cache.Model.DistributedLock, Cache.Model.DistributedLock?>;
