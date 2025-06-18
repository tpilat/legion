namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IPermissionRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.Permission>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Permission>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.Permission.IGetAllPermissionsWithRoles GetAllPermissionsWithRoles(
		Legion.ADF.Auth.Queries.Permission.GetAllPermissionsWithRolesQuery getAllPermissionsWithRolesQuery);

	Legion.ADF.Auth.Queries.Permission.IGetClaimsByRoleId GetClaimsByRoleId(
		Legion.ADF.Auth.Queries.Permission.GetClaimsByRoleIdQuery getClaimsByRoleIdQuery);

	Legion.ADF.Auth.Queries.Permission.IGetClaimsByUserId GetClaimsByUserId(
		Legion.ADF.Auth.Queries.Permission.GetClaimsByUserIdQuery getClaimsByUserIdQuery);

	Legion.ADF.Auth.Queries.Permission.IGetPermissionByClaimValue GetPermissionByClaimValue(
		Legion.ADF.Auth.Queries.Permission.GetPermissionByClaimValueQuery getPermissionByClaimValueQuery);

	Legion.ADF.Auth.Queries.Permission.IGetPermissionsByClaimValues GetPermissionsByClaimValues(
		Legion.ADF.Auth.Queries.Permission.GetPermissionsByClaimValuesQuery getPermissionsByClaimValuesQuery);

	Legion.ADF.Auth.Queries.Permission.IGetPermissionsByRoleId GetPermissionsByRoleId(
		Legion.ADF.Auth.Queries.Permission.GetPermissionsByRoleIdQuery getPermissionsByRoleIdQuery);

	Legion.ADF.Auth.Queries.Permission.IGetPermissionsByRoleIdAndClaimValue GetPermissionsByRoleIdAndClaimValue(
		Legion.ADF.Auth.Queries.Permission.GetPermissionsByRoleIdAndClaimValueQuery getPermissionsByRoleIdAndClaimValueQuery);
}
