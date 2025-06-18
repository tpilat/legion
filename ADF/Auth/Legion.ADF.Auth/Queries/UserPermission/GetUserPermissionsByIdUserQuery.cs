using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.UserPermission;

public record GetUserPermissionsByIdUserQuery(
	Guid IdUser,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserPermission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UserPermission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserPermission, List<Model.UserPermission>>;

public record GetValidUserPermissionsByIdUserQuery(
	Guid IdUser,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserPermission>>? QueryableBuilder = null)
	: GetUserPermissionsByIdUserQuery(IdUser, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserPermission, List<Model.UserPermission>>;
