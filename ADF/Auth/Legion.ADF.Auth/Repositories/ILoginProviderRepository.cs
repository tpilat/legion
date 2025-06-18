namespace Legion.ADF.Auth.Model.Repositories;

public partial interface ILoginProviderRepository : Legion.ADF.Auth.IAuthRepository<Legion.ADF.Auth.Model.LoginProvider>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.LoginProvider>? AccessControlManager { get; }

	Legion.ADF.Auth.Queries.LoginProvider.IGetLoginProviderByName GetLoginProviderByName(
		Legion.ADF.Auth.Queries.LoginProvider.GetLoginProviderByNameQuery getLoginProviderByNameQuery);
}
