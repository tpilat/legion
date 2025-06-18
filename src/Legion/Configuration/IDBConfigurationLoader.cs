namespace Legion.Configuration;

public interface IDBConfigurationLoader : IDisposable, IAsyncDisposable
{
	IDictionary<string, string?> LoadAllData(IScopeContext scopeContext);
}
