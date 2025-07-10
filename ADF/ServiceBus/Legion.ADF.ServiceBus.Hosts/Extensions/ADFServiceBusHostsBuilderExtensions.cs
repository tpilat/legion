//using Legion.ADF.ServiceBus.Hosts;
//using Legion.ADF.ServiceBus.Hosts.Services.Internal;
//using Microsoft.Extensions.DependencyInjection;

//namespace Legion.ADF.ServiceBus;

//public static class ADFServiceBusHostsBuilderExtensions
//{
//	public static ADFServiceBusHostsBuilder ConfigureHost(
//		this ADFServiceBusHostsBuilder builder)
//	{
//		Throw.IfArgumentNull(builder);

//		if (builder.EnterpriseServiceBusConfigured)
//			Throw.InvalidOperationException($"{nameof(builder.EnterpriseServiceBusConfigured)} already configured");

//		builder.EnterpriseServiceBusConfigured = true;

//		if (builder.EnterpriseServiceBusConfigured)
//			builder.ADFServiceBusBuilder.Services.AddHostedService<EnterpriseServiceBus>();

//		return builder;
//	}
//}
