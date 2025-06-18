using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Permission;

public record GetPermissionsByClaimValuesQuery(
	List<string> ClaimValues,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Permission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Model.Permission>>;
