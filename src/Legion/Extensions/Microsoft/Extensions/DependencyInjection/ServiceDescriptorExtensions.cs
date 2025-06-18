using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Legion.Extensions;

public static class ServiceDescriptorExtensions
{
	public static ServiceDescriptor WithImplementationFactory(this ServiceDescriptor descriptor, Func<IServiceProvider, object> implementationFactory) =>
		new(descriptor.ServiceType, implementationFactory, descriptor.Lifetime);

	public static ServiceDescriptor WithServiceType(this ServiceDescriptor descriptor, Type serviceType) => descriptor switch
	{
		{ ImplementationType: not null } => new ServiceDescriptor(serviceType, descriptor.ImplementationType, descriptor.Lifetime),
		{ ImplementationFactory: not null } => new ServiceDescriptor(serviceType, descriptor.ImplementationFactory, descriptor.Lifetime),
		{ ImplementationInstance: not null } => new ServiceDescriptor(serviceType, descriptor.ImplementationInstance),
		_ => throw new ArgumentException($"No implementation factory or instance or type found for {descriptor.ServiceType}.", nameof(descriptor))
	};

	/// <summary>
	/// Get all registered <see cref="ServiceDescriptor"/>
	/// </summary>
	/// <param name="provider"></param>
	/// <returns></returns>
	public static ServiceDescriptor[] GetAllServiceDescriptors(this IServiceProvider provider)
	{
		//var result = new Dictionary<Type, ServiceDescriptor>();

		var root = provider.GetType().GetProperty("RootProvider", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(provider);
		if (root != null)
			provider = (IServiceProvider)root;

		var callSiteFactory = provider?.GetType().GetProperty("CallSiteFactory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(provider);
		var descriptors = callSiteFactory?.GetType().GetProperty("Descriptors", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(callSiteFactory);

		return (descriptors as ServiceDescriptor[])!;

		//var descriptorLookup = callSiteFactory?.GetType().GetField("_descriptorLookup", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(callSiteFactory);
		//if (descriptorLookup is IDictionary dictionary)
		//{
		//	foreach (DictionaryEntry entry in dictionary)
		//	{
		//		var desc = (ServiceDescriptor)entry.Value?.GetType().GetProperty("Last", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(entry.Value)!;
		//		result.Add((Type)entry.Key, desc);
		//	}
		//}

		//return result;
	}
}
