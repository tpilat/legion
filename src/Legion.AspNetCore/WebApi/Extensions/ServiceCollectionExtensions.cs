using Legion.AspNetCore.Configurations;
using Legion.AspNetCore.WebApi.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Legion.Extensions;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection AddWebApiControllers(
		this IServiceCollection services,
		IConfiguration? configuration,
		IEnumerable<Assembly>? swaggerDocAssemblies = null,
		string configBindingPath = "WebApi")
	{
		Throw.IfArgumentNull(services);

		services.AddWebApiOptions(configBindingPath, swaggerDocAssemblies);

		Assembly[] assemblies = [
			typeof(AspNetCore.WebApi.ApiControllerBase).Assembly
		];

		//Add all validators from Legion.ADF.Logs.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		services.AddSwaggerGen();
		services.ConfigureOptions<ConfigureSwaggerOptions>();
		services.AddEndpointsApiExplorer();

		services.AddSingleton<IConfigureOptions<MvcOptions>, WebApiMvcOptions>();
		services.AddControllers();

		return services;
	}
}
