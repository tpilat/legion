namespace Legion.ADF.Auth.Model.Repositories;

public partial interface IVwUserRepository : Legion.ADF.Auth.IAuthQueryRepository<Legion.ADF.Auth.Model.VwUser>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Auth.Model.VwUser>? AccessControlManager { get; }

}
