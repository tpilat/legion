using Legion.MessageBus.Messages;

namespace Legion.ADF.Cache.Queries.CacheData;

public record GetCacheDataByKeyHashQuery(
	string KeyHash,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Cache.Model.CacheData>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.CacheData>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Cache.Model.CacheData, Cache.Model.CacheData?>;
