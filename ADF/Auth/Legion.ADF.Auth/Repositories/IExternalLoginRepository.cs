namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IExternalLoginRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.ExternalLogin>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.ExternalLogin>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.ExternalLogin.IGetExternalLoginByExternalIdentifier GetExternalLoginByExternalIdentifier(
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginByExternalIdentifierQuery getExternalLoginByExternalIdentifierQuery);

	Legion.ADF.Auth.Queries.ExternalLogin.IGetExternalLoginByUserAndExternalIdentifier GetExternalLoginByUserAndExternalIdentifier(
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginByUserAndExternalIdentifierQuery getExternalLoginByUserAndExternalIdentifierQuery);

	Legion.ADF.Auth.Queries.ExternalLogin.IGetExternalLoginsByUserId GetExternalLoginsByUserId(
		Legion.ADF.Auth.Queries.ExternalLogin.GetExternalLoginsByUserIdQuery getExternalLoginsByUserIdQuery);
}
