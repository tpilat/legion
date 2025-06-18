using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Audit;

public class ADFAuditBuilder
{
	public IConfiguration? Configuration { get; }
	public IServiceCollection Services { get; }

	public ADFAuditBuilder(IServiceCollection services, IConfiguration? configuration)
	{
		Throw.IfArgumentNull(services);

		Services = services;
		Configuration = configuration;
	}
}
