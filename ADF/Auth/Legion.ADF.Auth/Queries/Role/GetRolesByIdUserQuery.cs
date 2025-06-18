using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Role;

public record GetRolesByIdUserQuery(
	Guid IdUser,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Role>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, List<string>>;

public record GetValidRolesByIdUserQuery(
	Guid IdUser,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: GetRolesByIdUserQuery(IdUser, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, List<string>>;
