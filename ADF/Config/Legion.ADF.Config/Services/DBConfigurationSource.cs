using Legion.Configuration;
using Microsoft.Extensions.Configuration;

namespace Legion.ADF.Config;

public sealed class DBConfigurationSource : IConfigurationSource
{
	private readonly Func<IDBConfigurationLoader> _loaderDelegate;

	public DBConfigurationSource(Func<IDBConfigurationLoader> loaderDelegate)
	{
		Throw.IfArgumentNull(loaderDelegate);

		_loaderDelegate = loaderDelegate;
	}

	public IConfigurationProvider Build(IConfigurationBuilder builder) =>
		new DBConfigurationProvider(_loaderDelegate);
}
