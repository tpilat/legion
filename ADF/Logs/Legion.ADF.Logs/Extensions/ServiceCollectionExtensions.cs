using Legion.ADF.Logs.Services;
using Legion.ADF.Logs.Settings;
using Legion.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Legion.ADF.Logs.Extensions;

public static class ServiceCollectionExtensions
{
	public static ADFLogsBuilder AddADFLogs(
		this IServiceCollection services,
		IConfiguration? configuration = null,
		Action<ILoggingBuilder>? configure = null)
	{
		//settings / options
		services.AddAppSettings();

		Assembly[] assemblies = [
			typeof(ADFLogsBuilder).Assembly,
			typeof(LogsStore).Assembly
		];

		//Add all validators from Legion.ADF.Logs.Abstractions.dll
		services.AddValidators(ServiceLifetime.Singleton, assemblies);

		//add all TOption builders
		services.ConfigureOptionsBuilders(assemblies);

		if (configuration != null)
		{
			//add all service builders
			services.ConfigureServiceCollectionBuilders(configuration, assemblies);
		}

		services.TryAddTransient<BatchLogMessageStore>();
		services.TryAddTransient<BatchUnstructuredLogStore>();
		services.TryAddTransient<LogsStore>();

		services.AddLogging(loggingBuilder =>
		{
			loggingBuilder.AddProvider(sp => new ADFLoggerProvider(sp));

			//loggingBuilder.SetMinimumLevel(LogLevel.Trace);

			//LOG EVERYTHIG from ADFLoggerProvider
			loggingBuilder.AddFilter<ADFLoggerProvider>(null, LogLevel.Trace);

			configure?.Invoke(loggingBuilder);
		});

		return new ADFLogsBuilder(services, configuration);
	}
}
