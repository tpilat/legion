using Legion.Extensions;
using Legion.Logging.PostgreSQL.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Legion.Logging.PostgreSQL;

public static class PostgreSQLLoggerExtensions
{
	public static ILoggingBuilder AddPostgreSQLLogger(this ILoggingBuilder builder)
	{
		builder.AddConfiguration();

		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, PostgreSQLLoggerProvider>());

		//LoggerProviderOptions.RegisterProviderOptions
		//	<PostgreSQLLoggerConfiguration, PostgreSQLLoggerProvider>(builder.Services);

		builder.Services.AddValidators<PostgreSQLLoggerProvider>();

		builder.Services
			.AddOptions<PostgreSQLLoggerConfiguration>()
			.BindConfiguration("PostgreSQLLogger")
			.AddOptionsValidator("PostgreSQLLogger")
			.ValidateOnStart()
			;

		return builder;
	}

	public static ILoggingBuilder AddPostgreSQLLogger(
		this ILoggingBuilder builder,
		Action<PostgreSQLLoggerConfiguration> configure)
	{
		builder.AddPostgreSQLLogger();
		builder.Services.Configure(configure);

		return builder;
	}
}
