using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs;

public class ADFLogsBuilder
{
	public IConfiguration? Configuration { get; }
	public IServiceCollection Services { get; }

	public ADFLogsBuilder(IServiceCollection services, IConfiguration? configuration)
	{
		Throw.IfArgumentNull(services);

		Services = services;
		Configuration = configuration;
	}
}
