using Legion.MessageBus.Messages;
using System.Security.Claims;

namespace Legion.ADF.Auth.Queries.Permission;

public record GetClaimsByUserIdQuery(
	Guid IdUser,
	bool GetDeleted,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: Legion.Queries.BaseQuery<Model.Permission>(AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Claim>>;

public record GetValidClaimsByUserIdQuery(
	Guid IdUser,
	bool CheckReadPermissions,
	bool AsNoTracking = false,
	bool DisableCahce = true,
	Action<Legion.Queries.IQueryableBuilder<Model.Permission>>? QueryableBuilder = null)
	: GetClaimsByUserIdQuery(IdUser, false, CheckReadPermissions, AsNoTracking, DisableCahce, QueryableBuilder),
		IQueryRequest<Model.Permission, List<Claim>>;
