namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IUserPermissionRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.UserPermission>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.UserPermission>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.UserPermission.IGetUserPermissionsByIdUser GetUserPermissionsByIdUser(
		Legion.ADF.Auth.Queries.UserPermission.GetUserPermissionsByIdUserQuery getUserPermissionsByIdUserQuery);

	Legion.ADF.Auth.Queries.UserPermission.IGetUserPermissionsByIdUserAndClaimValue GetUserPermissionsByIdUserAndClaimValue(
		Legion.ADF.Auth.Queries.UserPermission.GetUserPermissionsByIdUserAndClaimValueQuery getUserPermissionsByIdUserAndClaimValueQuery);

	Legion.ADF.Auth.Queries.UserPermission.IGetUserPermissionsByIdUserAndClaimValues GetUserPermissionsByIdUserAndClaimValues(
		Legion.ADF.Auth.Queries.UserPermission.GetUserPermissionsByIdUserAndClaimValuesQuery getUserPermissionsByIdUserAndClaimValuesQuery);
}
