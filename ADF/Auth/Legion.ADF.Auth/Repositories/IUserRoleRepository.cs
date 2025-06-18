namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IUserRoleRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.UserRole>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.UserRole>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.UserRole.IGetUserRoleByIdUserAndIdRole GetUserRoleByIdUserAndIdRole(
		Legion.ADF.Auth.Queries.UserRole.GetUserRoleByIdUserAndIdRoleQuery getUserRoleByIdUserAndIdRole);

	Legion.ADF.Auth.Queries.UserRole.IGetUserRoleByIdUserAndNormalizedRoleName GetUserRoleByIdUserAndNormalizedRoleName(
		Legion.ADF.Auth.Queries.UserRole.GetUserRoleByIdUserAndNormalizedRoleNameQuery getUserRoleByIdUserAndNormalizedRoleName);

	Legion.ADF.Auth.Queries.UserRole.IIsInRole IsInRole(
		Legion.ADF.Auth.Queries.UserRole.IsInRoleQuery IsInRole);
}
