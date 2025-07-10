//using Legion.ADF.ServiceBus.Jobs;
//using Legion.ADF.ServiceBus.Jobs.Services;
//using Legion.ADF.ServiceBus.Jobs.Services.Internal;
//using Legion.Extensions;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using System.Reflection;

//namespace Legion.ADF.ServiceBus;

//public static class ADFServiceBusBuilderExtensions
//{
//	public static ADFEnterpriseServiceBusBuilder ConfigureJobs(
//		this ADFEnterpriseServiceBusBuilder builder,
//		Action<ADFServiceBusJobsBuilder> configure)
//	{
//		Throw.IfArgumentNull(builder);
//		Throw.IfArgumentNull(configure);

//		if (!builder.ADFServiceBusBuilderContext.AddJobs())
//			Throw.InvalidOperationException($"{nameof(Jobs)} already configured");

//		Assembly[] assemblies = [
//			typeof(JobsStore).Assembly
//		];

//		//Add all validators from Legion.ADF.ServiceBus.Jobs.dll
//		builder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

//		//add all TOption builders
//		builder.Services.ConfigureOptionsBuilders(assemblies);

//		if (builder.Configuration != null)
//		{
//			//add all service builders
//			builder.Services.ConfigureServiceCollectionBuilders(builder.Configuration, assemblies);
//		}

//		builder.Services.TryAddTransient<IJobsStore, JobsStore>();

//		builder.Services.AddStartupTask<JobsStartup>();

//		var adfServiceBusJobsBuilder = new ADFServiceBusJobsBuilder(builder);
//		configure.Invoke(adfServiceBusJobsBuilder);

//		return builder;
//	}

//	public static ADFServiceBusMonitorBuilder ConfigureJobs(
//		this ADFServiceBusMonitorBuilder builder,
//		Action<ADFServiceBusJobsBuilder> configure)
//	{
//		Throw.IfArgumentNull(builder);
//		Throw.IfArgumentNull(configure);

//		if (!builder.ADFServiceBusBuilderContext.AddJobs())
//			Throw.InvalidOperationException($"{nameof(Jobs)} already configured");

//		Assembly[] assemblies = [
//			typeof(IJobsMonitor).Assembly
//		];

//		//Add all validators from Legion.ADF.ServiceBus.Jobs.dll
//		builder.Services.AddValidators(ServiceLifetime.Singleton, assemblies);

//		//add all TOption builders
//		builder.Services.ConfigureOptionsBuilders(assemblies);

//		if (builder.Configuration != null)
//		{
//			//add all service builders
//			builder.Services.ConfigureServiceCollectionBuilders(builder.Configuration, assemblies);
//		}

//		builder.Services.TryAddTransient<IJobsStore, JobsStore>();

//		return builder;
//	}
//}
