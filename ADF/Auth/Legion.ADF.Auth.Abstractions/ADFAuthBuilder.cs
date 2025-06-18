using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Auth;

public class ADFAuthBuilder
{
	public IConfiguration? Configuration { get; }
	public IServiceCollection Services { get; }
	public bool AddRoles { get; }

	public ADFAuthBuilder(IServiceCollection services, IConfiguration? configuration, bool addRoles)
	{
		Throw.IfArgumentNull(services);

		Services = services;
		Configuration = configuration;
		AddRoles = addRoles;
	}
}
