using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Role;

public record GetAllRolesWithPermissionsQuery(
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Role>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, List<Model.Role>>;

public record GetAllValidRolesWithPermissionsQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: GetAllRolesWithPermissionsQuery(false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, List<Model.Role>>;
