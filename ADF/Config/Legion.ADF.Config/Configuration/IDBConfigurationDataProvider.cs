namespace Legion.ADF.Config.Configuration;

public interface IDBConfigurationDataProvider : IDisposable, IAsyncDisposable
{
	void Initialize(IScopeContext scopeContext, string connectionString);

	IDictionary<string, string?> LoadAllData(IScopeContext scopeContext);

	IDictionary<string, string?> GetDataByPath(IScopeContext scopeContext, string path);

	IDictionary<string, Legion.ADF.Config.Model.ConfigurationKeyValue> GetConfigurationKeyValuesStartWithPath(string path);

	void AddConfigurationKeyValue(Legion.ADF.Config.Model.ConfigurationKeyValue configurationKeyValue);

	void RemoveConfigurationKeyValue(Legion.ADF.Config.Model.ConfigurationKeyValue configurationKeyValue);

	int Save(IScopeContext scopeContext);
}
