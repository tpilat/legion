using Legion.MessageBus.Messages;

namespace Legion.ADF.Cache.Queries.ReloadableCacheKey;

public record GetReloadableCacheKeyByKeyQuery(
	string Key,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Cache.Model.ReloadableCacheKey>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.ReloadableCacheKey>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Cache.Model.ReloadableCacheKey, Cache.Model.ReloadableCacheKey?>;
