using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.UserRole;

public record GetUserRoleByIdUserAndIdRoleQuery(
	Guid IdUser,
	Guid IdRole,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserRole>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UserRole>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserRole, Model.UserRole?>;

public record GetValidUserRoleByIdUserAndIdRoleQuery(
	Guid IdUser,
	Guid IdRole,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserRole>>? QueryableBuilder = null)
	: GetUserRoleByIdUserAndIdRoleQuery(IdUser, IdRole, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserRole, Model.UserRole?>;
