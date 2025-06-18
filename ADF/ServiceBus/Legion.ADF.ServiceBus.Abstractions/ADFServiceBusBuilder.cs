using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus;

public class ADFServiceBusBuilder
{
	public IConfiguration? Configuration { get; }
	public IServiceCollection Services { get; }

	public ADFServiceBusBuilder(IServiceCollection services, IConfiguration? configuration)
	{
		Throw.IfArgumentNull(services);

		Services = services;
		Configuration = configuration;
	}
}
