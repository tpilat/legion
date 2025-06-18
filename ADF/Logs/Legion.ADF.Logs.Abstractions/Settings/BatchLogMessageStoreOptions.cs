using Legion.DataWriters;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs.Settings;

public class BatchLogMessageStoreOptions : BatchWriterOptions
{
	public class BatchLogMessageStoreOptionsValidator : ValidatorBase<BatchLogMessageStoreOptions>
	{
		public BatchLogMessageStoreOptionsValidator() { }
		public BatchLogMessageStoreOptionsValidator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<BatchLogMessageStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<BatchLogMessageStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.BatchSizeLimit, v => v.GreaterThan(0))
				.ForProperty(x => x.Period, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.MinimumBackoffPeriod, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.MaximumBackoffInterval, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.QueueLimit, v => v.GreaterThan(0))
				;
		}
	}
}

public static class BatchLogMessageStoreOptionsExtensions
{
	public static IServiceCollection AddBatchLogMessageStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<BatchLogMessageStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(BatchLogMessageStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(BatchLogMessageStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}
