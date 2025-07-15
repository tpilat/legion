using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Settings;

public class ServiceBusMonitorOptions
{
	public string? StoreId { get; set; }
	public string MonitorIdentifier { get; set; }

	public class Validator : ValidatorBase<ServiceBusMonitorOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ServiceBusMonitorOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ServiceBusMonitorOptions> builder)
		{
			builder?
				.ForProperty(x => x.MonitorIdentifier, v => v.NotDefaultOrWhiteSpace());
		}
	}
}

public static class ServiceBusMonitorOptionsExtensions
{
	public static IServiceCollection AddServiceBusMonitorOptions(
		this IServiceCollection services,
		string esbConfigBindingPath)
	{
		Throw.IfArgumentNullOrWhiteSpace(esbConfigBindingPath);

		services
			.AddOptions<ServiceBusMonitorOptions>()
			.BindConfiguration($"{esbConfigBindingPath}:{nameof(ServiceBusMonitorOptions)}")
			.AddOptionsValidator($"{esbConfigBindingPath}.{nameof(ServiceBusMonitorOptions)}")
			.ValidateOnStart();

		return services;
	}
}
