using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.User;

public record GetUsersByClaimValueQuery(
	string ClaimValue,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.User>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, List<Model.User>>;

public record GetValidUsersByClaimValueQuery(
	string ClaimValue,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: GetUsersByClaimValueQuery(ClaimValue, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, List<Model.User>>;
