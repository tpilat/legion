using Legion.Extensions;
using Legion.Infrastructure;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Settings;

public class OutboxMessageProcessingServiceOptions
{
	public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(60);
	public string? OutboxMessageProcessingServiceName { get; set; } = EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(Exceptions.Internal.ErrorCodes.OutboxMessageProcessingService);
	public string? OutboxMessageProcessingServiceVersion { get; set; } = EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion ?? "0.0.0.0";
	public int? MaxDegreeOfQueueParallelism { get; set; }
	public bool DisableMultiProcessingLog { get; set; } = true;
	public bool LogToStandardILogger { get; set; }
	public LogLevel LogLevel { get; set; } = LogLevel.Warning;

	public class Validator : ValidatorBase<OutboxMessageProcessingServiceOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<OutboxMessageProcessingServiceOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<OutboxMessageProcessingServiceOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdleTimeout, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.OutboxMessageProcessingServiceName, v => v.MaxLength(255))
				.ForProperty(x => x.OutboxMessageProcessingServiceVersion, v => v.MaxLength(15));
		}
	}
}

public static class OutboxMessageProcessingServiceOptionsExtensions
{
	public static IServiceCollection AddOutboxMessageProcessingServiceOptions(this IServiceCollection services)
	{
		services
			.AddOptions<OutboxMessageProcessingServiceOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(OutboxMessageProcessingServiceOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(OutboxMessageProcessingServiceOptions)}")
			.ValidateOnStart();

		return services;
	}
}