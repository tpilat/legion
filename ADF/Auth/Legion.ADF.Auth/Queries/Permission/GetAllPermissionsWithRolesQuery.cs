using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Permission;

public record GetAllPermissionsWithRolesQuery(
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Permission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Model.Permission>>;

public record GetAllValidPermissionsWithRolesQuery(
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: GetAllPermissionsWithRolesQuery(false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Model.Permission>>;
