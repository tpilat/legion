using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.Role;

public record GetRoleByNormalizedNameQuery(
	string NormalizedRoleName,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Role>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, Model.Role?>;

public record GetValidRoleByNormalizedNameQuery(
	string NormalizedRoleName,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Role>>? QueryableBuilder = null)
	: GetRoleByNormalizedNameQuery(NormalizedRoleName, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Role, Model.Role?>;
