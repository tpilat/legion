//using Legion.ADF.ServiceBus.Jobs.Services.Internal;
//using Microsoft.Extensions.DependencyInjection.Extensions;

//namespace Legion.ADF.ServiceBus.Jobs;

//public class ADFServiceBusJobsBuilder
//{
//	public ADFServiceBusBuilder ADFServiceBusBuilder { get; }
//	internal JobsRegistry JobsRegistry { get; }

//	public ADFServiceBusJobsBuilder(ADFServiceBusBuilder adfServiceBusBuilder)
//	{
//		Throw.IfArgumentNull(adfServiceBusBuilder);

//		ADFServiceBusBuilder = adfServiceBusBuilder;

//		JobsRegistry = new JobsRegistry();

//		ADFServiceBusBuilder.Services.TryAddSingleton(JobsRegistry);
//	}
//}
