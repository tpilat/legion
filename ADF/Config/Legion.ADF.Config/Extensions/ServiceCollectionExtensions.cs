using Legion.ADF.Config.Services;
using Legion.ADF.Config.Settings;
using Legion.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Legion.ADF.Config.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFConfigBuilder AddADFConfig(
		this IServiceCollection services,
		IConfiguration? configuration = null)
	{
		//settings / options
		services.AddAppSettings();

		Assembly[] assemblies = [
			typeof(ADFConfigBuilder).Assembly,
			typeof(ConfigStore).Assembly
		];

		//Add all validators from Legion.ADF.Config.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		services.TryAddTransient<ConfigStore>();

		return new ADFConfigBuilder(services, configuration);
	}
}
