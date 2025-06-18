using Legion.ADF.ESB.ServiceBus.Configuration;
using Legion.ADF.ESB.Settings;
using Legion.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ESB.ServiceBus.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddADFEngerpriseServiceBus(
		this IServiceCollection services,
		Action<ServiceBusConfiguration> configuration)
	{
		Throw.IfArgumentNull(configuration);

		//settings / options
		services.AddAppSettings();

		//Add all validators from AppSettings / Options
		services.AddValidators<AppSettings>();

		var cfg = new ServiceBusConfiguration();
		configuration.Invoke(cfg);

		return services;
	}
}
