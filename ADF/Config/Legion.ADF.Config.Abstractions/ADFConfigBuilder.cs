using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Config;

public class ADFConfigBuilder
{
	public IConfiguration? Configuration { get; }
	public IServiceCollection Services { get; }

	public ADFConfigBuilder(IServiceCollection services, IConfiguration? configuration)
	{
		Throw.IfArgumentNull(services);

		Services = services;
		Configuration = configuration;
	}
}
