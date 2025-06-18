using Legion.Validation;
using Legion.Validation.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.Extensions;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection AddValidators<TSearchBaseAssembly>(
		this IServiceCollection services,
		ServiceLifetime validatorLifetime = ServiceLifetime.Singleton)
		=> AddValidators(services, validatorLifetime, typeof(TSearchBaseAssembly).Assembly);

	public static IServiceCollection AddValidators(
		this IServiceCollection services,
		ServiceLifetime validatorLifetime = ServiceLifetime.Singleton,
		params Assembly[] assemblies)
	{
		Throw.IfArgumentNullOrEmpty(assemblies);

		AddLegionValidators(services, validatorLifetime, assemblies);
		AddValidatorDescriptorBuilders(services, assemblies);

		return services;
	}

	private static IServiceCollection AddLegionValidators(
		this IServiceCollection services,
		ServiceLifetime validatorLifetime = ServiceLifetime.Singleton,
		params Assembly[] assemblies)
	{
		Throw.IfArgumentNullOrEmpty(assemblies);

		var validatorType = typeof(IValidator<>);

		var types =
			assemblies
				.Distinct()
				.SelectMany(a => a.DefinedTypes)
				.Where(type =>
					!type.IsInterface
					&& !type.IsAbstract
					&& type.Implements(validatorType));

		foreach (var type in types)
		{
			var validatorIfc = type.GetInterfaces().FirstOrDefault(ifc => ifc.Implements(validatorType) || ifc.GetGenericTypeDefinitionIfExists() == validatorType);
			if (validatorIfc != null)
			{
				services.TryAdd(new ServiceDescriptor(validatorIfc, type, validatorLifetime));
				services.TryAdd(new ServiceDescriptor(type, type, validatorLifetime));
			}
		}

		return services;
	}

	private static IServiceCollection AddValidatorDescriptorBuilders(IServiceCollection services, params Assembly[] assemblies)
	{
		Throw.IfArgumentNullOrEmpty(assemblies);

		var validatorManager = new ValidatorManager();

		var validatorDescriptorBuilderType = typeof(IValidatorDescriptorBuilder);

		var typesToScan =
			assemblies
				.Distinct()
				.SelectMany(a => a.DefinedTypes)
				.Where(type =>
					!type.IsInterface
					&& !type.IsAbstract
					&& validatorDescriptorBuilderType.IsAssignableFrom(type));

		bool found = false;
		foreach (var descriptorBuilderTypeInfo in typesToScan)
		{
			var attribute = descriptorBuilderTypeInfo.GetCustomAttribute<ValidatorRegisterAttribute>();
			if (0 < attribute?.RegisteredTypes?.Length)
			{
				foreach (var commandType in attribute.RegisteredTypes)
				{
					IValidatorDescriptorBuilder? validatorDescriptorBuilder = null;

					var defaultCtor = descriptorBuilderTypeInfo.GetDefaultConstructor();
					if (defaultCtor == null)
					{
#if NET8_0_OR_GREATER
						validatorDescriptorBuilder = (IValidatorDescriptorBuilder)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(descriptorBuilderTypeInfo);
#else
						validatorDescriptorBuilder = (IValidatorDescriptorBuilder)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(descriptorBuilderTypeInfo);
#endif
						if (validatorDescriptorBuilder == null)
							throw new InvalidOperationException($"Cannot create instance of {descriptorBuilderTypeInfo}");
					}
					else
					{
						validatorDescriptorBuilder = (IValidatorDescriptorBuilder)defaultCtor.Invoke(null);
						if (validatorDescriptorBuilder == null)
							throw new InvalidOperationException($"Cannot create instance of {descriptorBuilderTypeInfo}");
					}

					found = validatorManager.RegisterValidatorDescriptorFor(validatorDescriptorBuilder.ObjectType, commandType, validatorDescriptorBuilder) || found;
				}
			}
		}

		//if (!found)
		//	throw new ConfigurationException("No validator was found.");

		services.TryAddSingleton<IValidatorManager>(validatorManager);

		return services;
	}
}
