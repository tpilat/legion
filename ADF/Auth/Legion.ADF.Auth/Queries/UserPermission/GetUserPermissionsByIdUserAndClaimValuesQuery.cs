using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.UserPermission;

public record GetUserPermissionsByIdUserAndClaimValuesQuery(
	Guid IdUser,
	List<string> ClaimValues,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserPermission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UserPermission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserPermission, List<Model.UserPermission>>;

public record GetValidUserPermissionsByIdUserAndClaimValuesQuery(
	Guid IdUser,
	List<string> ClaimValues,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserPermission>>? QueryableBuilder = null)
	: GetUserPermissionsByIdUserAndClaimValuesQuery(IdUser, ClaimValues, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserPermission, List<Model.UserPermission>>;
