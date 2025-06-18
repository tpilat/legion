namespace Legion.ADF.Config.Model.Repositories;

public partial interface IConfigurationClassRepository : Legion.ADF.Config.IConfigRepository<Legion.ADF.Config.Model.ConfigurationClass>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationClass>? AccessControlManager { get; }

	Legion.ADF.Config.Queries.ConfigurationClass.IGetAllConfigurationClasses GetAllConfigurationClasses(
		Legion.ADF.Config.Queries.ConfigurationClass.GetAllConfigurationClassesQuery getAllConfigurationClassesQuery);

	Legion.ADF.Config.Queries.ConfigurationClass.IGetConfigurationClassByRootPath GetConfigurationClassByRootPath(
		Legion.ADF.Config.Queries.ConfigurationClass.GetConfigurationClassByRootPathQuery getConfigurationClassByRootPathQuery);
}
