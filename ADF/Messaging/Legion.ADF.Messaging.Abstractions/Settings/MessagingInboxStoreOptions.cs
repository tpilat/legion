using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Settings;

public class MessagingInboxStoreOptions
{
	public string MessagingInboxStoreId { get; set; }
	public LogLevel LogLevel { get; set; } = LogLevel.Warning;

	public class Validator : ValidatorBase<MessagingInboxStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<MessagingInboxStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<MessagingInboxStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.MessagingInboxStoreId, v => v.MinLength(0));
		}
	}
}

public static class MessagingInboxStoreOptionsExtensions
{
	public static IServiceCollection AddMessagingInboxStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<MessagingInboxStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(MessagingInboxStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(MessagingInboxStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}