using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.ExternalLogin;

public record GetExternalLoginByUserAndExternalIdentifierQuery(
	Guid IdUser,
	string LoginProvider,
	string ExternalUserIdentifier,
	DateTime? ValidToUtc,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.ExternalLogin>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.ExternalLogin>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.ExternalLogin, Model.ExternalLogin?>;

public record GetValidExternalLoginByUserAndExternalIdentifierQuery(
	Guid IdUser,
	string LoginProvider,
	string ExternalUserIdentifier,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.ExternalLogin>>? QueryableBuilder = null)
	: GetExternalLoginByUserAndExternalIdentifierQuery(IdUser, LoginProvider, ExternalUserIdentifier, GlobalContext.Instance.UtcNow, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.ExternalLogin, Model.ExternalLogin?>;
