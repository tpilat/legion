using Legion.Bus;
using Legion.DependencyInjection;
using Legion.Exceptions.Internal;
using Legion.MessageBus;
using Legion.MessageBus.MessageResolvers.Internal;
using Legion.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Legion.Extensions;

public static partial class ServiceCollectionExtensions
{
	const string ConfigureServiceCollectionMethodName = "ConfigureServiceCollection";
	const string ConfigureOptionsMethodName = "ConfigureOptions";

	public static IServiceCollection AddInMemoryMessageBus(
		this IServiceCollection services,
		List<Assembly> handlerAndInterceptorAssembliesToScan,
		ServiceLifetime messageBusLifetime = ServiceLifetime.Scoped,
		ServiceLifetime handlerLifetime = ServiceLifetime.Transient,
		ServiceLifetime interceptorLifetime = ServiceLifetime.Transient)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNullOrEmpty(handlerAndInterceptorAssembliesToScan);

		services.TryAdd(new ServiceDescriptor(typeof(IMessageBus<>), typeof(InMemoryMessageBus<>), messageBusLifetime));

		var typeResolver = new FullNameTypeResolver();

		var messageHandlerRegistry = new MessageHandlerRegistry(
			services,
			typeResolver,
			handlerLifetime,
			interceptorLifetime);

		var eventHandlerRegistry = new EventHandlerRegistry(
			services,
			typeResolver,
			handlerLifetime,
			interceptorLifetime);

		var registeredAnyHandler = false;

		foreach (var assembly in handlerAndInterceptorAssembliesToScan)
		{
			var typesToScan =
				assembly.DefinedTypes
					.Where(type => type.IsInstanceable());

			foreach (var typeInfo in typesToScan)
			{
				var registered = messageHandlerRegistry.TryRegisterHandlerAndInterceptor(typeInfo);
				if (registered)
					registeredAnyHandler = true;

				registered = eventHandlerRegistry.TryRegisterHandlerAndInterceptor(typeInfo);
				if (registered)
					registeredAnyHandler = true;
			}
		}

		if (!registeredAnyHandler)
			Throw.InvalidOperationException(ErrorCodes.Bus.NoHandlerRegistered);

		return services;
	}

	public static IServiceCollection ConfigureOptionsBuilders(this IServiceCollection services,
		params Assembly[] serviceBuilderAssembliesToScan)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNullOrEmpty(serviceBuilderAssembliesToScan);

		var iServiceCollectionOptionsBuilderType = typeof(IServiceCollectionOptionsBuilder);
		var iServiceCollectionType = typeof(IServiceCollection);

		var types =
			serviceBuilderAssembliesToScan
				.Distinct()
				.SelectMany(a => a.DefinedTypes)
				.Where(type =>
					!type.IsInterface
					&& !type.IsAbstract
					&& type.Implements(iServiceCollectionOptionsBuilderType));

		foreach (var assembly in serviceBuilderAssembliesToScan)
		{
			var typesToScan =
				assembly.DefinedTypes
					.Where(type => type.IsInstanceable());

			foreach (var type in types)
			{
				var configureOptionsMethod = type.GetMethod(ConfigureOptionsMethodName, BindingFlags.Public | BindingFlags.Static);
				Throw.IfNull(configureOptionsMethod, (IErrorCode?)null, $"Type = {type.ToFriendlyFullName()} has no {ConfigureOptionsMethodName} method");

				var parameterTypes = configureOptionsMethod.GetParameters()?.Select(x => x.ParameterType).ToList();
				if (parameterTypes?.Count != 1)
					Throw.InvalidOperationException($"Method {ConfigureOptionsMethodName} must have 1 parameter of type {iServiceCollectionType.ToFriendlyFullName()}");

				if (parameterTypes[0] != iServiceCollectionType)
					Throw.InvalidOperationException($"Method {ConfigureOptionsMethodName} must have 1 parameter of type {iServiceCollectionType.ToFriendlyFullName()}");

				configureOptionsMethod.Invoke(null, [services]);
			}
		}

		return services;
	}

	public static IServiceCollection ConfigureServiceCollectionBuilders(this IServiceCollection services,
		IConfiguration configuration,
		params Assembly[] serviceBuilderAssembliesToScan)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNullOrEmpty(serviceBuilderAssembliesToScan);

		var iServiceCollectionBuilderType = typeof(IServiceCollectionBuilder);
		var iServiceCollectionType = typeof(IServiceCollection);
		var iConfigurationType = typeof(IConfiguration);

		var types =
			serviceBuilderAssembliesToScan
				.Distinct()
				.SelectMany(a => a.DefinedTypes)
				.Where(type =>
					!type.IsInterface
					&& !type.IsAbstract
					&& type.Implements(iServiceCollectionBuilderType));

		foreach (var assembly in serviceBuilderAssembliesToScan)
		{
			var typesToScan =
				assembly.DefinedTypes
					.Where(type => type.IsInstanceable());

			foreach (var type in types)
			{
				var configureServiceCollectionMethod = type.GetMethod(ConfigureServiceCollectionMethodName, BindingFlags.Public | BindingFlags.Static);
				Throw.IfNull(configureServiceCollectionMethod, (IErrorCode?)null, $"Type = {type.ToFriendlyFullName()} has no {ConfigureServiceCollectionMethodName} method");

				var parameterTypes = configureServiceCollectionMethod.GetParameters()?.Select(x => x.ParameterType).ToList();
				if (parameterTypes?.Count != 2)
					Throw.InvalidOperationException($"Method {ConfigureServiceCollectionMethodName} must have 2 parameters of type {iServiceCollectionType.ToFriendlyFullName()} and {iConfigurationType.ToFriendlyFullName()}");

				if (parameterTypes[0] != iServiceCollectionType)
					Throw.InvalidOperationException($"Method {ConfigureServiceCollectionMethodName} must have 2 parameters of type {iServiceCollectionType.ToFriendlyFullName()} and {iConfigurationType.ToFriendlyFullName()}");

				if (parameterTypes[1] != iConfigurationType)
					Throw.InvalidOperationException($"Method {ConfigureServiceCollectionMethodName} must have 2 parameters of type {iServiceCollectionType.ToFriendlyFullName()} and {iConfigurationType.ToFriendlyFullName()}");

				configureServiceCollectionMethod.Invoke(null, [services, configuration]);
			}
		}

		return services;
	}

	public static OptionsBuilder<TOptions> AddAndConfigureOptions<TOptions>(
		this IServiceCollection services,
		Action<OptionsBuilder<TOptions>>? builder,
		Action<IServiceProvider, TOptions> configure,
		bool addOptionsValidator = true,
		string? validatorBasePath = null,
		bool validateOnStart = true)
		where TOptions : class
	{
		var optionsBuilder = services.AddOptions<TOptions>();

		builder?.Invoke(optionsBuilder);

		services.AddSingleton<IConfigureOptions<TOptions>>(sp =>
		{
			var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
			return new OptionsWithServiceProvider<TOptions>(serviceScopeFactory, configure);
		});

		if (addOptionsValidator)
			optionsBuilder.AddOptionsValidator(validatorBasePath);

		if (validateOnStart)
			optionsBuilder
				.ValidateOnStart();

		return optionsBuilder;
	}
}
