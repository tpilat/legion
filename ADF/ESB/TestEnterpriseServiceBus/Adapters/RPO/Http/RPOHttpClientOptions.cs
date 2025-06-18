using Legion;
using Legion.ADF.ESB.Components;
using Legion.DependencyInjection;
using Legion.Extensions;
using Legion.NetHttp;
using Legion.Reflection.ObjectPaths;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestEnterpriseServiceBus.Adapters.RPO.Http;

public class RPOHttpClientOptions : HttpApiClientOptions, IServiceCollectionOptionsBuilder, IServiceCollectionBuilder
{
	const string BASE_CONFIG_PATH = "TestEnterpriseServiceBusConfig:RPOClientAdapterConfig";

	public static IServiceCollection ConfigureOptions(IServiceCollection services)
	{
		Throw.IfArgumentNull(services);

		services.TryAddTransient<AdapterRequestResponseLogger>();

		services
			.AddAndConfigureOptions<RPOHttpClientOptions>(
				b => b.BindConfiguration($"{BASE_CONFIG_PATH}:{nameof(RPOHttpClientOptions)}"),
				(sp, o) =>
				{
					o.SetDefaultRequestResponseLogger<AdapterRequestResponseLogger, Guid?>();

					o.TrustToAllServerCertificates = true;
					o.UsesCookieContainerToStoreServerCookies = true;
				},
				true,
				BASE_CONFIG_PATH,
				true);

		return services;
	}

	public static IServiceCollection ConfigureServiceCollection(IServiceCollection services, IConfiguration configuration)
	{
		Throw.IfArgumentNull(services);
		Throw.IfArgumentNull(configuration);

		services.AddHttpApiClient<RPOHttpClient, RPOHttpClientOptions, Guid?>();

		return services;
	}

	public class RPOHttpClientOptionsValidator : Legion.NetHttp.HttpApiClientOptions.Validator<RPOHttpClientOptions>
	{
		public RPOHttpClientOptionsValidator() { }
		public RPOHttpClientOptionsValidator(IObjectPath objectPath) : base(objectPath) { }

		//public override void SetDefaultRuels(ValidatorBuilder<RPOHttpClientOptions> builder)
		//	=> RPOHttpClientOptionsRulesBuilder(builder);

		//public static void RPOHttpClientOptionsRulesBuilder(ValidatorBuilder<RPOHttpClientOptions> builder)
		//	=> RulesBuilder(builder);
	}
}
