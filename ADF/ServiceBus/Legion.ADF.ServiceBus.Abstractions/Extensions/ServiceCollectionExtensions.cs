using Legion.ADF.ServiceBus.Settings;
using Legion.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Legion.ADF.ServiceBus;

public static class ServiceCollectionExtensions
{
	public static ADFServiceBusBuilder AddADFEnterpriseServiceBus(
		this IServiceCollection services,
		string esbConfigBindingPath,
		IConfiguration? configuration = null)
	{
		//settings / options
		services.AddAppSettings(esbConfigBindingPath);

		Assembly[] assemblies = [
			typeof(ADFServiceBusBuilder).Assembly
		];

		//Add all validators from Legion.ADF.ServiceBus.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		return new ADFServiceBusBuilder(services, configuration);
	}

	public static ADFServiceBusMonitorBuilder AddADFServiceBusMonitor(
		this IServiceCollection services,
		string esbConfigBindingPath,
		IConfiguration? configuration = null)
	{
		//settings / options
		services.AddAppSettings(esbConfigBindingPath);

		Assembly[] assemblies = [
			typeof(ADFServiceBusMonitorBuilder).Assembly
		];

		//Add all validators from Legion.ADF.ServiceBus.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		return new ADFServiceBusMonitorBuilder(services, configuration);
	}
}
