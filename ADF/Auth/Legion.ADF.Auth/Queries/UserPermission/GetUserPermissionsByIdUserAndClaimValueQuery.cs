using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.UserPermission;

public record GetUserPermissionsByIdUserAndClaimValueQuery(
	Guid IdUser,
	string ClaimValue,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserPermission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UserPermission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserPermission, List<Model.UserPermission>>;

public record GetValidUserPermissionsByIdUserAndClaimValueQuery(
	Guid IdUser,
	string ClaimValue,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserPermission>>? QueryableBuilder = null)
	: GetUserPermissionsByIdUserAndClaimValueQuery(IdUser, ClaimValue, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserPermission, List<Model.UserPermission>>;

