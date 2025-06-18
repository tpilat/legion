using Legion.Exceptions.Internal;
using Microsoft.Extensions.Configuration;

namespace Legion.Extensions;

public static class IConfigurationExtensions
{
	public static IConfigurationSection? GetSectionPath(this IConfiguration configuration, string path)
	{
		Throw.IfArgumentNull(configuration);
		Throw.IfArgumentNullOrWhiteSpace(path);

		IConfigurationSection? result = null;
		var split = path.Split('.');
		foreach (var key in split)
		{
			if (result == null)
			{
				result = configuration.GetSection(key);
			}
			else
			{
				result = result.GetSection(key);
			}

		}

		return result;
	}

	public static IConfigurationSection GetRequiredSectionPath(this IConfiguration configuration, string path)
	{
		Throw.IfArgumentNull(configuration);
		Throw.IfArgumentNullOrWhiteSpace(path);

		IConfigurationSection? result = null;
		var split = path.Split('.');
		var currentPath = "";
		foreach (var key in split)
		{
			if (result == null)
			{
				currentPath = $"{currentPath}.{key}";
				result = configuration.GetSection(key);
				if (result == null)
					Throw.ConfigurationException(ErrorCodes.ConfigurationException.MissingConfiguratoinSection(currentPath));
			}
			else
			{
				currentPath = $"{currentPath}.{key}";
				result = result.GetSection(key);
				if (result == null)
					Throw.ConfigurationException(ErrorCodes.ConfigurationException.MissingConfiguratoinSection(currentPath));
			}

		}
		
		if (result == null)
			Throw.ConfigurationException(ErrorCodes.ConfigurationException.MissingConfiguratoinSection(path));

		return result;
	}
}
