using Legion.Extensions;
using Legion.NetHttp;
using Legion.Reflection.ObjectPaths;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.ServiceBus.RestApi.Client;

public class ServiceBusRestApiClientOptions : HttpApiClientOptions
{
	public string BaseAddress { get; set; }

	public class Validator : ValidatorBase<ServiceBusRestApiClientOptions>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<ServiceBusRestApiClientOptions> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<ServiceBusRestApiClientOptions> builder)
		{
			//builder?
			//	.ForProperty(x => x.BaseAddress, v => v.NotDefaultOrWhiteSpace());
		}
	}
}

public static class ServiceBusRestApiClientOptionsExtensions
{
	public static IServiceCollection AddServiceBusRestApiClientOptions(this IServiceCollection services, string configBindingPath)
	{
		Throw.IfArgumentNullOrWhiteSpace(configBindingPath);

		services
			.AddOptions<ServiceBusRestApiClientOptions>()
			.BindConfiguration($"{configBindingPath}")
			.AddOptionsValidator($"{configBindingPath}")
			.ValidateOnStart();

		return services;
	}
}
