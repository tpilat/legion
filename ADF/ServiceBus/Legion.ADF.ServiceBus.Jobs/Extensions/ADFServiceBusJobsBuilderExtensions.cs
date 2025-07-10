//using Legion.ADF.ServiceBus.Jobs;

//namespace Legion.ADF.ServiceBus;

//public static class ADFServiceBusJobsBuilderExtensions
//{
//	public static ADFServiceBusJobsBuilder AddJob<T>(
//		this ADFServiceBusJobsBuilder builder,
//		IScopeContext scopeContext)
//	{
//		scopeContext = scopeContext.CreateNew();

//		Throw.IfArgumentNull(builder);

//		builder.JobsRegistry.RegisterJob<T>(scopeContext);

//		return builder;
//	}
//}
