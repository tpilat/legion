using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Settings;

public class DomainEventProcessingServiceOptions
{
	public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(60);
	public int MessagesBatchCount { get; set; } = 10;
	public TimeSpan NextProcessingTimeout { get; set; } = TimeSpan.FromSeconds(60);
	public int MaxRetryCount { get; set; } = 5;
	public int? MaxDegreeOfParallelism { get; set; } //must be in contrast with MessagesBatchCount
	public bool DisableProcessingLog { get; set; }
	public bool DisableMultiProcessingLog { get; set; } = true;

	public class Validator : ValidatorBase<DomainEventProcessingServiceOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<DomainEventProcessingServiceOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<DomainEventProcessingServiceOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdleTimeout, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.MessagesBatchCount, v => v.GreaterThan(0))
				.ForProperty(x => x.NextProcessingTimeout, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.MaxRetryCount, v => v.GreaterThan(0))
				.ForProperty(x => x.MaxDegreeOfParallelism, v => v.GreaterThan(0));
		}
	}
}

public static class DomainEventProcessingServiceOptionsExtensions
{
	public static IServiceCollection AddDomainEventProcessingServiceOptions(this IServiceCollection services)
	{
		services
			.AddOptions<DomainEventProcessingServiceOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(DomainEventProcessingServiceOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(DomainEventProcessingServiceOptions)}")
			.ValidateOnStart();

		return services;
	}
}