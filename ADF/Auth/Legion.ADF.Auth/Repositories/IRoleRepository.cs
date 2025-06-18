namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IRoleRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.Role>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.Role>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.Role.IGetAllRolesWithPermissions GetAllRolesWithPermissions(
		Legion.ADF.Auth.Queries.Role.GetAllRolesWithPermissionsQuery getAllRolesWithPermissionsQuery);

	Legion.ADF.Auth.Queries.Role.IGetRoleById GetRoleById(
		Legion.ADF.Auth.Queries.Role.GetRoleByIdQuery getRoleByIdQuery);

	Legion.ADF.Auth.Queries.Role.IGetRoleByNormalizedName GetRoleByNormalizedName(
		Legion.ADF.Auth.Queries.Role.GetRoleByNormalizedNameQuery getRoleByNormalizedNameQuery);

	Legion.ADF.Auth.Queries.Role.IGetRolesByIdUser GetRolesByIdUser(
		Legion.ADF.Auth.Queries.Role.GetRolesByIdUserQuery getRolesByIdUserQuery);
}
