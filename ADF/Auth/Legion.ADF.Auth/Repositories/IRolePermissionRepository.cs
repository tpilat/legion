namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IRolePermissionRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.RolePermission>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.RolePermission>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.RolePermission.IGetRolePermissionsByRoleIdAndClaimValue GetRolePermissionsByRoleIdAndClaimValue(
		Legion.ADF.Auth.Queries.RolePermission.GetRolePermissionsByRoleIdAndClaimValueQuery getRolePermissionsByRoleIdAndClaimValueQuery);
}
