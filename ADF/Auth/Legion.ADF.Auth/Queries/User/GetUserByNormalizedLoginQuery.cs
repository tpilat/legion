using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.User;

public record GetUserByNormalizedLoginQuery(
	string NormalizedLogin,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.User>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;

public record GetValidUserByNormalizedLoginQuery(
	string NormalizedLogin,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: GetUserByNormalizedLoginQuery(NormalizedLogin, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;
