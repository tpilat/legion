using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Permission;

public record GetPermissionsByRoleIdQuery(
	Guid IdRole,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Permission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Model.Permission>>;

public record GetValidPermissionsByRoleIdQuery(
	Guid IdRole,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: GetPermissionsByRoleIdQuery(IdRole, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Model.Permission>>;
