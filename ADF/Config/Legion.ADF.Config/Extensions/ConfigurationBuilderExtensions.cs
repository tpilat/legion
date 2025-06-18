using Legion.Configuration;
using Microsoft.Extensions.Configuration;

namespace Legion.ADF.Config.Extensions;

public static class ConfigurationBuilderExtensions
{
	public static IConfigurationBuilder AddDBConfiguration(this IConfigurationBuilder builder, Func<IDBConfigurationLoader> loaderDelegate)
		=> builder.Add(new DBConfigurationSource(loaderDelegate));
}
