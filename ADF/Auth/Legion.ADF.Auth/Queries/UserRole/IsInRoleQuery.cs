using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.UserRole;

public record IsInRoleQuery(
	Guid IdUser,
	string NormalizedRoleName,
	bool IncludeDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserRole>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UserRole>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserRole, bool>;

public record IsInValidRoleQuery(
	Guid IdUser,
	string NormalizedRoleName,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserRole>>? QueryableBuilder = null)
	: IsInRoleQuery(IdUser, NormalizedRoleName, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserRole, bool>;
