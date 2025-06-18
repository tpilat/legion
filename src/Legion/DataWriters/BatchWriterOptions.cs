using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.DataWriters;

public class BatchWriterOptions : IBatchWriterOptions
{
	/// <summary>
	/// Eagerly write the first received event to the database to check, if database table is ready
	/// </summary>
	public bool EagerlyEmitFirstEvent { get; set; } = true;

	/// <summary>
	/// Max events count in one batch written do database
	/// </summary>
	public int BatchSizeLimit { get; set; } = 1000;

	/// <summary>
	/// Flush period even if BatchSizeLimit is not exceeded
	/// </summary>
	public TimeSpan Period { get; set; } = TimeSpan.FromMilliseconds(20);

	/// <summary>
	/// Minimum delay to retry, if database insert fails
	/// </summary>
	public TimeSpan MinimumBackoffPeriod { get; set; } = TimeSpan.FromSeconds(5);

	/// <summary>
	/// Maximum delay to retry, if database insert fails
	/// </summary>
	public TimeSpan MaximumBackoffInterval { get; set; } = TimeSpan.FromMinutes(10);

	/// <summary>
	/// If QueueLimit is exceeded, new events will be dropped
	/// </summary>
	public int? QueueLimit { get; set; } = 100000;

	public class Validator : ValidatorBase<BatchWriterOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<BatchWriterOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<BatchWriterOptions> builder)
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

public static class BatchWriterOptionsExtensions
{
	public static IServiceCollection AddBatchWriterOptions(
		this IServiceCollection services,
		Action<Microsoft.Extensions.Options.OptionsBuilder<BatchWriterOptions>> bindConfiguration,
		string validatorBasePath)
	{
		Throw.IfArgumentNull(bindConfiguration);
		Throw.IfArgumentNullOrWhiteSpace(validatorBasePath);

		var optionsBuilder = services.AddOptions<BatchWriterOptions>();

		//optionsBuilder.BindConfiguration(string configSectionPath)
		bindConfiguration.Invoke(optionsBuilder);

		optionsBuilder
			.AddOptionsValidator(validatorBasePath)
			.ValidateOnStart();

		return services;
	}
}
