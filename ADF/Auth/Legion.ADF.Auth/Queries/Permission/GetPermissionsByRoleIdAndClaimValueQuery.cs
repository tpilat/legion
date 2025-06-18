using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Permission;

public record GetPermissionsByRoleIdAndClaimValueQuery(
	Guid IdRole,
	string ClaimValue,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Permission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Guid>>;

public record GetValidPermissionsByRoleIdAndClaimValueQuery(
	Guid IdRole,
	string ClaimValue,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: GetPermissionsByRoleIdAndClaimValueQuery(IdRole, ClaimValue, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Guid>>;
