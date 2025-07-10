using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.Settings;

public class EnterpriseServiceBusOptions
{
	public string? ServiceName { get; set; }
	public string HostName { get; set; }
	public string? StoreId { get; set; }
	public int NoHostTimeoutInSeconds { get; set; }

	public class Validator : ValidatorBase<EnterpriseServiceBusOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<EnterpriseServiceBusOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<EnterpriseServiceBusOptions> builder)
		{
			builder?
				.ForProperty(x => x.HostName, v => v.NotDefaultOrWhiteSpace())
				.ForProperty(x => x.NoHostTimeoutInSeconds, v => v.GreaterThan(0));
		}
	}
}

public static class EnterpriseServiceBusOptionsExtensions
{
	public static IServiceCollection AddEnterpriseServiceBusOptions(
		this IServiceCollection services,
		string esbConfigBindingPath)
	{
		Throw.IfArgumentNullOrWhiteSpace(esbConfigBindingPath);

		services
			.AddOptions<EnterpriseServiceBusOptions>()
			.BindConfiguration($"{esbConfigBindingPath}:{nameof(EnterpriseServiceBusOptions)}")
			.AddOptionsValidator($"{esbConfigBindingPath}.{nameof(EnterpriseServiceBusOptions)}")
			.ValidateOnStart();

		return services;
	}
}
