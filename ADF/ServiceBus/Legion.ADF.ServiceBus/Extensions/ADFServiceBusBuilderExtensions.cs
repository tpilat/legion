using Legion.ADF.Cache.RestApi.Client;
using Legion.ADF.ServiceBus.Services.Internal;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Legion.ADF.ServiceBus;

public static class ADFServiceBusBuilderExtensions
{
	public static ADFServiceBusBuilder ConfigureEnterpriseServiceBus(
		this ADFServiceBusBuilder builder)
	{
		Throw.IfArgumentNull(builder);

		if (!builder.ADFServiceBusBuilderContext.Configured())
			Throw.InvalidOperationException($"{nameof(ServiceBus)} already configured");

		Assembly[] assemblies = [
			typeof(EnterpriseServiceBus).Assembly
		];

		//Add all validators from Legion.ADF.ServiceBus.dll
		builder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		builder.Services.ConfigureOptionsBuilders(assemblies);

		if (builder.Configuration != null)
		{
			//add all service builders
			builder.Services.ConfigureServiceCollectionBuilders(builder.Configuration, assemblies);
		}

		builder.Services.AddCacheRestApiClient("CacheRestApi");

		builder.Services.AddHostedService<EnterpriseServiceBus>();

		return builder;
	}

	//public static ADFServiceBusMonitorBuilder ConfigureHosts(
	//	this ADFServiceBusMonitorBuilder builder,
	//	Action<ADFServiceBusHostsBuilder> configure)
	//{
	//	Throw.IfArgumentNull(builder);
	//	Throw.IfArgumentNull(configure);

	//	if (!builder.ADFServiceBusBuilderContext.AddHosts())
	//		Throw.InvalidOperationException($"{nameof(Hosts)} already configured");

	//	Assembly[] assemblies = [
	//		typeof(IHostsMonitor).Assembly
	//	];

	//	//Add all validators from Legion.ADF.ServiceBus.dll
	//	builder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

	//	//add all TOption builders
	//	builder.Services.ConfigureOptionsBuilders(assemblies);

	//	if (builder.Configuration != null)
	//	{
	//		//add all service builders
	//		builder.Services.ConfigureServiceCollectionBuilders(builder.Configuration, assemblies);
	//	}

	//	//builder.Services.TryAddTransient<IHostsStore, HostsStore>();

	//	return builder;
	//}
}
