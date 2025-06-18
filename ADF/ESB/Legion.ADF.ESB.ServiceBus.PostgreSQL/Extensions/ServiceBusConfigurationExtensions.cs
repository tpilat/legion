using Legion.ADF.Config.PostgreSQL.Extensions;
using Legion.ADF.ESB.Components.PostgreSQL.Extensions;
using Legion.ADF.ESB.MBox.PostgreSQL.Extensions;
using Legion.ADF.ESB.Orchestrations.PostgreSQL.Extensions;
using Legion.ADF.ESB.ServiceBus.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ESB.ServiceBus.PostgreSQL;

public static class ServiceBusConfigurationExtensions
{
	public static void ConfigurePostgreSQL(
		this ServiceBusConfiguration configuration,
		IServiceCollection services)
	{
		Throw.IfArgumentNull(configuration);
		Throw.IfArgumentNull(services);

		services.AddConfigContexts<ConnectionStringProvider>();
		services.AddComponentsContexts<ConnectionStringProvider>();
		services.AddMBoxContexts<ConnectionStringProvider>();
		services.AddOrchestrationsContexts<ConnectionStringProvider>();
	}
}
