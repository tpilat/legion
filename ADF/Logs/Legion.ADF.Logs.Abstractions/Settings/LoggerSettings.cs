using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Logs.Settings;

public class LoggerSettings
{
	public bool UseBatchWriter { get; set; } = true;
	public LogLevel LogMessageMinLogLevel { get; set; }
	public LogLevel UnstructuredLogMinLogLevel { get; set; }

	public LoggerSettings()
	{
		LogMessageMinLogLevel = LogLevel.Trace;
		UnstructuredLogMinLogLevel = LogLevel.Warning;
	}

	public class Validator : ValidatorBase<LoggerSettings>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<LoggerSettings> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<LoggerSettings> builder)
		{
		}
	}
}

public static class LoggerSettingsExtensions
{
	public static IServiceCollection AddLoggerSettings(this IServiceCollection services)
	{
		services
			.AddOptions<LoggerSettings>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(LoggerSettings)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(LoggerSettings)}")
			.ValidateOnStart();

		return services;
	}
}

