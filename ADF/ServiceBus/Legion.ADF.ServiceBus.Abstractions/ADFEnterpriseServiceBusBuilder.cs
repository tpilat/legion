//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace Legion.ADF.ServiceBus;

//public class ADFEnterpriseServiceBusBuilder
//{
//	public ADFServiceBusBuilderContext ADFServiceBusBuilderContext { get; }

//	public IConfiguration? Configuration { get; }
//	public IServiceCollection Services { get; }

//	public ADFEnterpriseServiceBusBuilder(IServiceCollection services, IConfiguration? configuration)
//	{
//		Throw.IfArgumentNull(services);

//		Services = services;
//		Configuration = configuration;
//		ADFServiceBusBuilderContext = new ADFServiceBusBuilderContext();
//	}
//}
