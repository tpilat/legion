using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.ExternalLogin;

public record GetExternalLoginsByUserIdQuery(
	Guid IdUser,
	DateTime? ValidToUtc,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.ExternalLogin>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.ExternalLogin>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.ExternalLogin, List<Model.ExternalLogin>>;

public record GetValidExternalLoginsByUserIdQuery(
	Guid IdUser,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.ExternalLogin>>? QueryableBuilder = null)
	: GetExternalLoginsByUserIdQuery(IdUser, GlobalContext.Instance.UtcNow, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.ExternalLogin, List<Model.ExternalLogin>>;
