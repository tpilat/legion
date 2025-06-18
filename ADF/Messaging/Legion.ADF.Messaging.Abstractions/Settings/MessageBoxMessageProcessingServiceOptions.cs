using Legion.Extensions;
using Legion.Infrastructure;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Settings;

public class MessageBoxMessageProcessingServiceOptions
{
	public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(60);
	public string? MessageProcessingServiceName { get; set; } = EnvironmentInfoProviderCache.Instance.EntryAssemblyName ?? nameof(Exceptions.Internal.ErrorCodes.MessageBoxProcessingService);
	public string? MessageProcessingServiceVersion { get; set; } = EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion ?? "0.0.0.0";
	public int? MaxDegreeOfQueueParallelism { get; set; }
	public int? MaxDegreeOfTopicParallelism { get; set; }
	public bool DisableMultiProcessingLog { get; set; } = true;
	public bool LogToStandardILogger { get; set; }
	public LogLevel LogLevel { get; set; } = LogLevel.Warning;

	public class Validator : ValidatorBase<MessageBoxMessageProcessingServiceOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<MessageBoxMessageProcessingServiceOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<MessageBoxMessageProcessingServiceOptions> builder)
		{
			builder?
				.ForProperty(x => x.IdleTimeout, v => v.GreaterThan(TimeSpan.Zero))
				.ForProperty(x => x.MessageProcessingServiceName, v => v.MaxLength(255))
				.ForProperty(x => x.MessageProcessingServiceVersion, v => v.MaxLength(15));
		}
	}
}

public static class MessageBoxMessageProcessingServiceOptionsExtensions
{
	public static IServiceCollection AddMessageBoxMessageProcessingServiceOptions(this IServiceCollection services)
	{
		services
			.AddOptions<MessageBoxMessageProcessingServiceOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(MessageBoxMessageProcessingServiceOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(MessageBoxMessageProcessingServiceOptions)}")
			.ValidateOnStart();

		return services;
	}
}
