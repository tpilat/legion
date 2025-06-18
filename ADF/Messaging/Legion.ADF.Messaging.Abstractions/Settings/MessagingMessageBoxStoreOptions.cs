using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Legion.ADF.Messaging.Settings;

public class MessagingMessageBoxStoreOptions
{
	public string MessagingMessageBoxStoreId { get; set; }
	public LogLevel LogLevel { get; set; } = LogLevel.Warning;

	public class Validator : ValidatorBase<MessagingMessageBoxStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<MessagingMessageBoxStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<MessagingMessageBoxStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.MessagingMessageBoxStoreId, v => v.MinLength(0));
		}
	}
}

public static class MessagingMessageBoxStoreOptionsExtensions
{
	public static IServiceCollection AddMessagingMessageBoxStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<MessagingMessageBoxStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(MessagingMessageBoxStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(MessagingMessageBoxStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}
