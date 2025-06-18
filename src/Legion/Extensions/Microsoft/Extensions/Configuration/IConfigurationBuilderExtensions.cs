using Legion.Configuration;
using Microsoft.Extensions.Configuration;

namespace Legion.Extensions;

public static class IConfigurationBuilderExtensions
{
	public static IConfigurationBuilder AddConfigurationData(this IConfigurationBuilder builder, IDictionary<string, string?> data)
		=> builder.Add(new DictionaryConfigurationSource(data));
}
