using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Settings;

public class MessagingOutboxStoreOptions
{
	public string MessagingOutboxStoreId { get; set; }
	public LogLevel LogLevel { get; set; } = LogLevel.Warning;

	public class Validator : ValidatorBase<MessagingOutboxStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<MessagingOutboxStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<MessagingOutboxStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.MessagingOutboxStoreId, v => v.MinLength(0));
		}
	}
}

public static class MessagingOutboxStoreOptionsExtensions
{
	public static IServiceCollection AddMessagingOutboxStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<MessagingOutboxStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(MessagingOutboxStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(MessagingOutboxStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}