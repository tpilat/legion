using Legion.Configuration;
using Microsoft.Extensions.Configuration;

namespace Legion.ADF.Config;

public sealed class DBConfigurationProvider : ConfigurationProvider, IConfigurationProvider
{
	private readonly Func<IDBConfigurationLoader> _loaderDelegate;

	public DBConfigurationProvider(Func<IDBConfigurationLoader> loaderDelegate)
	{
		Throw.IfArgumentNull(loaderDelegate);

		_loaderDelegate = loaderDelegate;
	}

	public override void Load()
	{
		Data = _loaderDelegate().LoadAllData(ScopeContext.Create(nameof(DBConfigurationProvider)));
	}
}
