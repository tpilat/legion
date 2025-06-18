using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Settings;

public class MessagingDomainEventsStoreOptions
{
	public string MessagingDomainEventsStoreId { get; set; }

	public class Validator : ValidatorBase<MessagingDomainEventsStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<MessagingDomainEventsStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<MessagingDomainEventsStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.MessagingDomainEventsStoreId, v => v.MinLength(0));
		}
	}
}

public static class MessagingDomainEventsStoreOptionsExtensions
{
	public static IServiceCollection AddMessagingDomainEventsStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<MessagingDomainEventsStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(MessagingDomainEventsStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(MessagingDomainEventsStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}