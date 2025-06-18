using Legion.Extensions;
using Legion.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.ESB.Resources.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddLegionESBResources(this IServiceCollection services)
	{
		var resourceLocalizerFactoryType = typeof(IResourceLocalizer<>);

		var resourceLocalizerTypes =
			typeof(ServiceCollectionExtensions)
			.Assembly
			.DefinedTypes
			.Where(type =>
				!type.IsInterface
				&& !type.IsAbstract
				&& type.InheritsOrImplements(resourceLocalizerFactoryType))
			.ToList();

		Localizers.ResourceKeyLocalizerTypes = resourceLocalizerTypes.ToDictionary(
			x => x
				.GetInterfaces()
					.First(y => y.GetGenericTypeDefinitionIfExists() == resourceLocalizerFactoryType)
					.GetGenericArguments()[0],
			x => (Type)x);

		foreach (var type in resourceLocalizerTypes)
			services.TryAddSingleton((Type)type);

		return services;
	}
}
