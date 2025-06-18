namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IUserTokenRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.UserToken>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.UserToken>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.UserToken.IGetUserTokenByUserProviderTokenName GetUserTokenByUserProviderTokenName(
		Legion.ADF.Auth.Queries.UserToken.GetUserTokenByUserProviderTokenNameQuery getUserTokenByUserProviderTokenNameQuery);
}
