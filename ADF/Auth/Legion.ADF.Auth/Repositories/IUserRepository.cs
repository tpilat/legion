namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IUserRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.User>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.User>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.User.IGetUserByExternalLoginProviderIdentifier GetUserByExternalLoginProviderIdentifier(
		Legion.ADF.Auth.Queries.User.GetUserByExternalLoginProviderIdentifierQuery getUserByExternalLoginProviderIdentifierQuery);

	Legion.ADF.Auth.Queries.User.IGetUserById GetUserById(
		Legion.ADF.Auth.Queries.User.GetUserByIdQuery getUserByIdQuery);

	Legion.ADF.Auth.Queries.User.IGetUserByNormalizedEmail GetUserByNormalizedEmail(
		Legion.ADF.Auth.Queries.User.GetUserByNormalizedEmailQuery getUserByNormalizedEmailQuery);

	Legion.ADF.Auth.Queries.User.IGetUserByNormalizedLogin GetUserByNormalizedLogin(
		Legion.ADF.Auth.Queries.User.GetUserByNormalizedLoginQuery getUserByNormalizedLoginQuery);

	Legion.ADF.Auth.Queries.User.IGetUserByNormalizedRoleName GetUserByNormalizedRoleName(
		Legion.ADF.Auth.Queries.User.GetUserByNormalizedRoleNameQuery getUserByNormalizedRoleNameQuery);

	Legion.ADF.Auth.Queries.User.IGetUserPermissionsAndRolesById GetUserPermissionsAndRolesById(
		Legion.ADF.Auth.Queries.User.GetUserPermissionsAndRolesByIdQuery getUserPermissionsAndRolesByIdQuery);

	Legion.ADF.Auth.Queries.User.IGetUsersByClaimValue GetUsersByClaimValue(
		Legion.ADF.Auth.Queries.User.GetUsersByClaimValueQuery getUsersByClaimValueQuery);
}
