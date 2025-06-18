using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Role;

public record GetRoleByIdQuery(
	Guid IdRole,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Role>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, Model.Role?>;

public record GetValidRoleByIdQuery(
	Guid IdRole,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: GetRoleByIdQuery(IdRole, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, Model.Role?>;
