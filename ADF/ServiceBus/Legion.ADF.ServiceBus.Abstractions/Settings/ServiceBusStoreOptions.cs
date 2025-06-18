using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Settings;

public class ServiceBusStoreOptions
{
	public string ServiceBusStoreId { get; set; }

	public class Validator : ValidatorBase<ServiceBusStoreOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ServiceBusStoreOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ServiceBusStoreOptions> builder)
		{
			builder?
				.ForProperty(x => x.ServiceBusStoreId, v => v.MinLength(0));
		}
	}
}

public static class ServiceBusStoreOptionsExtensions
{
	public static IServiceCollection AddServiceBusStoreOptions(this IServiceCollection services)
	{
		services
			.AddOptions<ServiceBusStoreOptions>()
			.BindConfiguration($"{AppSettings.PREFIX}:{nameof(ServiceBusStoreOptions)}")
			.AddOptionsValidator($"{AppSettings.PREFIX}.{nameof(ServiceBusStoreOptions)}")
			.ValidateOnStart();

		return services;
	}
}