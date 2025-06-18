using Legion.MessageBus.Messages;

namespace Legion.ADF.Auth.Queries.LoginProvider;

public record GetLoginProviderByNameQuery(
	string Name,
	bool GetDisabledProviders,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.LoginProvider>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.LoginProvider>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.LoginProvider, Model.LoginProvider?>;

public record GetValidLoginProviderByNameQuery(
	string Name,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.LoginProvider>>? QueryableBuilder = null)
	: GetLoginProviderByNameQuery(Name, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.LoginProvider, Model.LoginProvider?>;
