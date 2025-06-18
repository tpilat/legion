using Legion.Extensions;
using Legion.Infrastructure;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Settings;

public class InboxMessageProcessingServiceOptions
{
	public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(60);
	public string? InboxMessageProcessingServiceName { get; set; } = EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(Exceptions.Internal.ErrorCodes.InboxMessageProcessingService);
	public string? InboxMessageProcessingServiceVersion { get; set; } = EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion ?? "0.0.0.0";
	public int? MaxDegreeOfQueueParallelism { get; set; }
	public bool DisableMultiProcessingLog { get; set; } = true;
	public bool LogToStandardILogger { get; set; }
	public LogLevel LogLevel { get; set; } = LogLevel.Warning;

	public class Validator : ValidatorBase<InboxMessageProcessingServiceOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<InboxMessageProcessingServiceOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<InboxMessageProcessingServiceOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdleTimeout, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.InboxMessageProcessingServiceName, v => v.MaxLength(255))
				.ForProperty(x => x.InboxMessageProcessingServiceVersion, v => v.MaxLength(15));
		}
	}
}

public static class InboxMessageProcessingServiceOptionsExtensions
{
	public static IServiceCollection AddInboxMessageProcessingServiceOptions(this IServiceCollection services)
	{
		services
			.AddOptions<InboxMessageProcessingServiceOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(InboxMessageProcessingServiceOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(InboxMessageProcessingServiceOptions)}")
			.ValidateOnStart();

		return services;
	}
}