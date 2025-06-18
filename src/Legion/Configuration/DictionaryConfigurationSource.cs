using Microsoft.Extensions.Configuration;

namespace Legion.Configuration;

public sealed class DictionaryConfigurationSource : IConfigurationSource
{
	private readonly IDictionary<string, string?> _config;

	public DictionaryConfigurationSource(IDictionary<string, string?> config)
	{
		Throw.IfArgumentNull(config);

		_config = config;
	}

	public IConfigurationProvider Build(IConfigurationBuilder builder) =>
		new DictionaryConfigurationProvider(_config);
}
