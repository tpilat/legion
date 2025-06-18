namespace Legion.ADF.Config.Model.Repositories;

public partial interface IConfigurationKeyValueRepository : Legion.ADF.Config.IConfigRepository<Legion.ADF.Config.Model.ConfigurationKeyValue>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Config.Model.ConfigurationKeyValue>? AccessControlManager { get; }

	Legion.ADF.Config.Queries.ConfigurationKeyValue.IGetAllConfigurationKeyValues GetAllConfigurationKeyValues(
		Legion.ADF.Config.Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesQuery getAllConfigurationKeyValuesQuery);

	Legion.ADF.Config.Queries.ConfigurationKeyValue.IGetAllConfigurationKeyValuesByPath GetAllConfigurationKeyValuesByPath(
		Legion.ADF.Config.Queries.ConfigurationKeyValue.GetAllConfigurationKeyValuesByPathQuery getAllConfigurationKeyValuesByPath);

	Legion.ADF.Config.Queries.ConfigurationKeyValue.IGetConfigurationKeyValueByKey GetConfigurationKeyValueByKey(
		Legion.ADF.Config.Queries.ConfigurationKeyValue.GetConfigurationKeyValueByKeyQuery getConfigurationKeyValueByKey);
}
