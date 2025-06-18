using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.UserRole;

public record GetUserRoleByIdUserAndNormalizedRoleNameQuery(
	Guid IdUser,
	string NormalizedRoleName,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserRole>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UserRole>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserRole, Model.UserRole?>;

public record GetValidUserRoleByIdUserAndNormalizedRoleNameQuery(
	Guid IdUser,
	string NormalizedRoleName,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserRole>>? QueryableBuilder = null)
	: GetUserRoleByIdUserAndNormalizedRoleNameQuery(IdUser, NormalizedRoleName, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserRole, Model.UserRole?>;
