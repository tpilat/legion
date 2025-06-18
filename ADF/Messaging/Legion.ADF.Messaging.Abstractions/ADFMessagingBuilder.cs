using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging;

public class ADFMessagingBuilder
{
	public ADFMessagingBuilderContext ADFMessagingBuilderContext { get; }

	public IConfiguration? Configuration { get; }
	public IServiceCollection Services { get; }

	public ADFMessagingBuilder(IServiceCollection services, IConfiguration? configuration)
	{
		Throw.IfArgumentNull(services);

		Services = services;
		Configuration = configuration;
		ADFMessagingBuilderContext = new ADFMessagingBuilderContext();
	}
}
