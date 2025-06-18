using Legion.DataWriters;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Logs.Settings;

public class BatchUnstructuredLogStoreOptions : BatchWriterOptions
{
	public class BatchUnstructuredLogStoreOptionsValidator : ValidatorBase<BatchUnstructuredLogStoreOptions>
	{
		public BatchUnstructuredLogStoreOptionsValidator() { }
		public BatchUnstructuredLogStoreOptionsValidator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<BatchUnstructuredLogStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<BatchUnstructuredLogStoreOptions> builder)
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

public static class BatchUnstructuredLogStoreOptionsExtensions
{
	public static IServiceCollection AddBatchUnstructuredLogStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<BatchUnstructuredLogStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(BatchUnstructuredLogStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(BatchUnstructuredLogStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}
