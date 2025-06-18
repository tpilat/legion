namespace Legion.ADF.Config.Model.Repositories;

public partial interface IVwConfigurationClassRepository : Legion.ADF.Config.IConfigQueryRepository<Legion.ADF.Config.Model.VwConfigurationClass>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.VwConfigurationClass>? AccessControlManager { get; }

}
