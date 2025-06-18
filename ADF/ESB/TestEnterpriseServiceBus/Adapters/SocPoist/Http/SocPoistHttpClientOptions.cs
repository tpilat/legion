using Legion;
using Legion.ADF.ESB.Components;
using Legion.DependencyInjection;
using Legion.Extensions;
using Legion.NetHttp;
using Legion.Reflection.ObjectPaths;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestEnterpriseServiceBus.Adapters.SocPoist.Http;

public class SocPoistHttpClientOptions : HttpApiClientOptions, IServiceCollectionOptionsBuilder, IServiceCollectionBuilder
{
	const string BASE_CONFIG_PATH = "TestEnterpriseServiceBusConfig:SocPoistClientAdapterConfig";

	public static IServiceCollection ConfigureOptions(IServiceCollection services)
	{
		Throw.IfArgumentNull(services);

		services.TryAddTransient<AdapterRequestResponseLogger>();

		services
			.AddAndConfigureOptions<SocPoistHttpClientOptions>(
				b => b.BindConfiguration($"{BASE_CONFIG_PATH}:{nameof(SocPoistHttpClientOptions)}"),
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

		services.AddHttpApiClient<SocPoistHttpClient, SocPoistHttpClientOptions, Guid?>(nameof(SocPoistHttpClient));

		return services;
	}

	public class SocPoistHttpClientOptionsValidator : Legion.NetHttp.HttpApiClientOptions.Validator<SocPoistHttpClientOptions>
	{
		public SocPoistHttpClientOptionsValidator() { }
		public SocPoistHttpClientOptionsValidator(IObjectPath objectPath) : base(objectPath) { }

		//public override void SetDefaultRuels(ValidatorBuilder<SocPoistHttpClientOptions> builder)
		//	=> SocPoistHttpClientOptionsRulesBuilder(builder);

		//public static void SocPoistHttpClientOptionsRulesBuilder(ValidatorBuilder<SocPoistHttpClientOptions> builder)
		//	=> RulesBuilder(builder);
	}
}
