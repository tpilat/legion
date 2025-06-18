using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.User;

public record GetUserByExternalLoginProviderIdentifierQuery(
	string LoginProvider,
	string ExternalUserIdentifier,
	DateTime? DeletedOrValidToUtc,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.User>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;

public record GetValidUserByExternalLoginProviderIdentifierQuery(
	string LoginProvider,
	string ExternalUserIdentifier,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.User>>? QueryableBuilder = null)
	: GetUserByExternalLoginProviderIdentifierQuery(LoginProvider, ExternalUserIdentifier, GlobalContext.Instance.UtcNow, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.User, Model.User?>;
