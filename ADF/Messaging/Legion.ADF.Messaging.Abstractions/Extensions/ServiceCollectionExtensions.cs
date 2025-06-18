using Legion.ADF.Messaging.Settings;
using Legion.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Legion.ADF.Messaging;

public static class ServiceCollectionExtensions
{
	public static ADFMessagingBuilder AddADFMessaging(
		this IServiceCollection services,
		IConfiguration? configuration = null)
	{
		//settings / options
		services.AddAppSettings();

		Assembly[] assemblies = [
			typeof(ADFMessagingBuilder).Assembly
		];

		//Add all validators from Legion.ADF.Messaging.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		return new ADFMessagingBuilder(services, configuration);
	}
}
