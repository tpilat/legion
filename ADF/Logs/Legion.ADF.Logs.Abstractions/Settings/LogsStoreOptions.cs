using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs.Settings;

public class LogsStoreOptions
{
	public string LogStoreId { get; set; }

	public class Validator : ValidatorBase<LogsStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<LogsStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<LogsStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.LogStoreId, v => v.MinLength(0));
		}
	}
}

public static class LogsStoreOptionsExtensions
{
	public static IServiceCollection AddLogsStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<LogsStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(LogsStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(LogsStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}