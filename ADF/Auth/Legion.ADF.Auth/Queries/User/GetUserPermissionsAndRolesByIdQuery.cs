using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.User;

public record GetUserPermissionsAndRolesByIdQuery(
	Guid IdUser,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.User>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;

public record GetValidUserPermissionsAndRolesByIdQuery(
	Guid IdUser,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: GetUserPermissionsAndRolesByIdQuery(IdUser, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;
