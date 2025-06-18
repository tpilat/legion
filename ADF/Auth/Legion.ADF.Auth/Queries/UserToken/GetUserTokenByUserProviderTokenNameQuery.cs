using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.UserToken;

public record GetUserTokenByUserProviderTokenNameQuery(
	Guid IdUser,
	string LoginProvider,
	string TokenName,
	DateTime? ValidToUtc,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserToken>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.UserToken>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserToken, Model.UserToken?>;

public record GetValidUserTokenByUserProviderTokenNameQuery(
	Guid IdUser,
	string LoginProvider,
	string TokenName,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.UserToken>>? QueryableBuilder = null)
	: GetUserTokenByUserProviderTokenNameQuery(IdUser, LoginProvider, TokenName, GlobalContext.Instance.UtcNow, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.UserToken, Model.UserToken?>;
