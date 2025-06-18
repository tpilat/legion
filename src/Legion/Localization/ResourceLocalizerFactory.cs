using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Legion.Localization;

public static class ResourceLocalizerFactory
{
	public static IStringLocalizer Create(IServiceProvider serviceProvider, string baseName, string assemblyName)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNullOrWhiteSpace(baseName);
		Throw.IfArgumentNullOrWhiteSpace(assemblyName);

		var stringLocalizerFactory = serviceProvider.GetRequiredService<IStringLocalizerFactory>();
		return stringLocalizerFactory.Create(baseName, assemblyName);
	}
}
