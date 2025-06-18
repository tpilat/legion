namespace Legion.Configuration;

public interface IDBConfigurationManager : IDBConfigurationLoader, IDisposable, IAsyncDisposable
{
	IDictionary<string, string?> GetDataByPath(IScopeContext scopeContext, string path);

	int SaveDataByPath(IScopeContext scopeContext, string path, IDictionary<string, string?> data, bool force, bool removeUnusedKeys);
}
