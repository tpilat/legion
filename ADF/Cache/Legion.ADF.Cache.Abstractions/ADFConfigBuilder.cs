using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Cache;

public class ADFCacheBuilder
{
	public IConfiguration? Configuration { get; }
	public IServiceCollection Services { get; }

	public ADFCacheBuilder(IServiceCollection services, IConfiguration? configuration)
	{
		Throw.IfArgumentNull(services);

		Services = services;
		Configuration = configuration;
	}
}
