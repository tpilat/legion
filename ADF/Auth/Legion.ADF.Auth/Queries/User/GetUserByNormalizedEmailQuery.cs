using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.User;

public record GetUserByNormalizedEmailQuery(
	string NormalizedEmail,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.User>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;

public record GetValidUserByNormalizedEmailQuery(
	string NormalizedEmail,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: GetUserByNormalizedEmailQuery(NormalizedEmail, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;
