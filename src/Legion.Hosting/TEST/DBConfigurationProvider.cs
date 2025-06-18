using Microsoft.Extensions.Configuration;

namespace Legion.Hosting.TEST;

internal class DBConfigurationProvider : ConfigurationProvider
{
	public override void Load()
	{
		base.Load();
	}

	public override bool TryGet(string key, out string? value)
	{
		return base.TryGet(key, out value);
	}

	public override void Set(string key, string? value)
	{
		base.Set(key, value);
	}

	public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
	{
		return base.GetChildKeys(earlierKeys, parentPath);
	}
}
