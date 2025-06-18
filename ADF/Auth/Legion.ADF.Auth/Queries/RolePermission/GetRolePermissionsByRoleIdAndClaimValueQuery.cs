using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.RolePermission;

public record GetRolePermissionsByRoleIdAndClaimValueQuery(
	Guid IdRole,
	string ClaimValue,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.RolePermission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.RolePermission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.RolePermission, List<Model.RolePermission>>;

public record GetValidRolePermissionsByRoleIdAndClaimValueQuery(
	Guid IdRole,
	string ClaimValue,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.RolePermission>>? QueryableBuilder = null)
	: GetRolePermissionsByRoleIdAndClaimValueQuery(IdRole, ClaimValue, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.RolePermission, List<Model.RolePermission>>;
