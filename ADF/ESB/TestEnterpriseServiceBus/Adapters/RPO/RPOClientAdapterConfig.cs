using Legion;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.ServiceBus.Initializers;
using Legion.DependencyInjection;
using Legion.Extensions;
using Legion.Reflection.ObjectPaths;
using Legion.Serializer;
using Legion.Transactions;
using Legion.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestEnterpriseServiceBus.Adapters.RPO.Http;

namespace TestEnterpriseServiceBus.Adapters.RPO;

public class RPOClientAdapterConfig : ESBAdapterConfig, IESBAdapterConfig, IServiceCollectionOptionsBuilder
{
	const string BASE_CONFIG_PATH = "TestEnterpriseServiceBusConfig";

	public RPOHttpClientOptions RPOHttpClientOptions { get; private set; }

	public RPOClientAdapterConfig()
	{
		RPOHttpClientOptions = new RPOHttpClientOptions();
	}

	public class Validator : ValidatorBase<RPOClientAdapterConfig>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<RPOClientAdapterConfig> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<RPOClientAdapterConfig> builder)
		{
			builder?
				.ForProperty(x => x.RPOHttpClientOptions, v => v.NotNull())
				.ForNavigation(
					x => x.RPOHttpClientOptions,
					RPOHttpClientOptions.RPOHttpClientOptionsValidator.RulesBuilder)
			;
		}
	}

	public static IServiceCollection ConfigureOptions(IServiceCollection services)
	{
		Throw.IfArgumentNull(services);

		services
			.AddAndConfigureOptions<RPOClientAdapterConfig>(
				b => b.BindConfiguration($"{BASE_CONFIG_PATH}:{nameof(RPOClientAdapterConfig)}"),
				(sp, o) =>
				{
					if (ESBInitializer.ConfigsInitializationStatus != ESBInitializationStatus.Finished)
					{
						o.SetDefaultOptions();

						//var path = $"{BASE_CONFIG_PATH}:{nameof(RPOClientAdapterConfig)}";
						//var json = JsonSerializerHelper.Serialize(o);
						//var dict = Legion.Configuration.JsonConfigurationParser_Newtonsoft.Parse(json, path, null, false);
						//var mngr = new Legion.ADF.Config.ConfigModel.PostgreSQL.Configuration.DBConfigurationManager(
						//		opt => opt.UseNpgsql(string.Format("Host=host.docker.internal;Database=legion_adf_esb;Port=35432;Username=legion_adf_esb_usr;Password=legion_adf_esb_pwd", System.Environment.GetEnvironmentVariable("PGPASSWORD"))),
						//		sp.GetRequiredService<ILogger<Legion.ADF.Config.ConfigModel.PostgreSQL.ConfigDbContext>>());

						//mngr.SaveDataByPath(ScopeContext.Create(nameof(RPOClientAdapterConfig)), path, dict, false, true);

						return;
					}

					var transactionsController = new TransactionsController();

					var scopeContext = ScopeContext.Create(nameof(RPOClientAdapterConfig));
					using var invocationContext =
						new InvocationContextBuilder(scopeContext)
						.Initialize(sp, transactionsController, false)
						.Build();

					var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
					var logger = loggerFactory.CreateLogger<RPOClientAdapterConfig>();

					var componentsUowResult = invocationContext.CreateUnitOfWork<IComponentsUnitOfWork, Legion.ADF.ESB.ConnectionStringProvider>();
					var componentsUoW = componentsUowResult.Data!;

					var dbProperties =
						componentsUoW.AdapterRepository
							.AsQueryable(scopeContext)
							.Where(x => x.IdAdapter == RPOClientAdapter.ADAPTER_ID)
							.Select(x => x.Properties)
							.First();

					var result = o.Merge(scopeContext, dbProperties);

					result.ThrowIfError(
						scopeContext,
						Legion.Exceptions.Internal.ErrorCodes.ConfigurationException.InvalidConfigMessage("Cannot merge config"),
						true,
						logger,
						skipIfAlreadyLogged: true,
						logWarnings: true);
				},
				true,
				BASE_CONFIG_PATH,
				true);

		return services;
	}

	public override RPOClientAdapterConfig GetDefaultOptions()
	{
		var result = new RPOClientAdapterConfig();
		result.SetDefaultOptions();
		return result;
	}

	public override void SetDefaultOptions()
	{
		MinLogLevel = Microsoft.Extensions.Logging.LogLevel.Warning;
		RPOHttpClientOptions = new RPOHttpClientOptions
		{
			ClientName = "RPOHttpClient",
			SourceSystemName = "RPOHttpClient",
			UserAgent = "RPOHttpClient",
			Version = new Version(1, 0, 0, 0),
			BaseAddress = "https://api.statistics.sk/rpo/v1/",
			DefaultTimeoutInSeconds = 60,
			LogRequest = true,
			LogRequestPayload = true,
			LogResponse = true,
			LogResponsePayload = true,
			TrustToAllServerCertificates = true,
			UsesCookieContainerToStoreServerCookies = true
		};
	}

	public override IResult Merge(IScopeContext scopeContext, string? savedProperties)
	{
		var result = new ResultBuilder();

		if (string.IsNullOrWhiteSpace(savedProperties))
			return result.Build();

		try
		{
			var savedOptions = JsonSerializerHelper.Deserialize<RPOClientAdapterConfig>(savedProperties);
			if (savedOptions == null)
				return result.WithInvalidOperationException(scopeContext, null, x => x.InternalMessage($"{nameof(savedOptions)} == null"));

			MinLogLevel = ValueGetterHelper.GetNewValueIfSet(MinLogLevel, savedOptions.MinLogLevel);

			if (savedOptions.RPOHttpClientOptions == null)
				return result.Build();

			RPOHttpClientOptions.ClientName = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.ClientName, savedOptions.RPOHttpClientOptions.ClientName);
			RPOHttpClientOptions.SourceSystemName = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.SourceSystemName, savedOptions.RPOHttpClientOptions.SourceSystemName);
			RPOHttpClientOptions.UserAgent = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.UserAgent, savedOptions.RPOHttpClientOptions.UserAgent);
			RPOHttpClientOptions.Version = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.Version, savedOptions.RPOHttpClientOptions.Version);
			RPOHttpClientOptions.BaseAddress = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.BaseAddress, savedOptions.RPOHttpClientOptions.BaseAddress);
			RPOHttpClientOptions.DefaultTimeoutInSeconds = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.DefaultTimeoutInSeconds, savedOptions.RPOHttpClientOptions.DefaultTimeoutInSeconds);
			RPOHttpClientOptions.LogRequest = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.LogRequest, savedOptions.RPOHttpClientOptions.LogRequest);
			RPOHttpClientOptions.LogRequestPayload = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.LogRequestPayload, savedOptions.RPOHttpClientOptions.LogRequestPayload);
			RPOHttpClientOptions.LogResponse = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.LogResponse, savedOptions.RPOHttpClientOptions.LogResponse);
			RPOHttpClientOptions.LogResponsePayload = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.LogResponsePayload, savedOptions.RPOHttpClientOptions.LogResponsePayload);
			RPOHttpClientOptions.TrustToAllServerCertificates = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.TrustToAllServerCertificates, savedOptions.RPOHttpClientOptions.TrustToAllServerCertificates);
			RPOHttpClientOptions.UsesCookieContainerToStoreServerCookies = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.UsesCookieContainerToStoreServerCookies, savedOptions.RPOHttpClientOptions.UsesCookieContainerToStoreServerCookies);
			RPOHttpClientOptions.StaticQueryStrings = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.StaticQueryStrings, savedOptions.RPOHttpClientOptions.StaticQueryStrings);
			RPOHttpClientOptions.ForceStaticQueryStrings = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.ForceStaticQueryStrings, savedOptions.RPOHttpClientOptions.ForceStaticQueryStrings);
			RPOHttpClientOptions.StaticHeaders = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.StaticHeaders, savedOptions.RPOHttpClientOptions.StaticHeaders);
			RPOHttpClientOptions.StaticHeaderCollections = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.StaticHeaderCollections, savedOptions.RPOHttpClientOptions.StaticHeaderCollections);
			RPOHttpClientOptions.StaticCookies = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.StaticCookies, savedOptions.RPOHttpClientOptions.StaticCookies);
			RPOHttpClientOptions.StaticFormData = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.StaticFormData, savedOptions.RPOHttpClientOptions.StaticFormData);
			RPOHttpClientOptions.AutomaticDecompression = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.AutomaticDecompression, savedOptions.RPOHttpClientOptions.AutomaticDecompression);
			RPOHttpClientOptions.WebProxySettings = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.WebProxySettings, savedOptions.RPOHttpClientOptions.WebProxySettings);
			RPOHttpClientOptions.CheckCertificateRevocationList = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.CheckCertificateRevocationList, savedOptions.RPOHttpClientOptions.CheckCertificateRevocationList);
			RPOHttpClientOptions.UseDefaultCredentials = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.UseDefaultCredentials, savedOptions.RPOHttpClientOptions.UseDefaultCredentials);
			RPOHttpClientOptions.SendAuthorizationHeaderInRequest = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.SendAuthorizationHeaderInRequest, savedOptions.RPOHttpClientOptions.SendAuthorizationHeaderInRequest);
			RPOHttpClientOptions.SslProtocols = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.SslProtocols, savedOptions.RPOHttpClientOptions.SslProtocols);
			RPOHttpClientOptions.MaxResponseHeadersLength = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.MaxResponseHeadersLength, savedOptions.RPOHttpClientOptions.MaxResponseHeadersLength);
			RPOHttpClientOptions.MaxRequestContentBufferSize = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.MaxRequestContentBufferSize, savedOptions.RPOHttpClientOptions.MaxRequestContentBufferSize);
			RPOHttpClientOptions.MaxConnectionsPerServer = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.MaxConnectionsPerServer, savedOptions.RPOHttpClientOptions.MaxConnectionsPerServer);
			RPOHttpClientOptions.MaxAutomaticRedirections = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.MaxAutomaticRedirections, savedOptions.RPOHttpClientOptions.MaxAutomaticRedirections);
			RPOHttpClientOptions.AllowAutoRedirect = ValueGetterHelper.GetNewValueIfSet(RPOHttpClientOptions.AllowAutoRedirect, savedOptions.RPOHttpClientOptions.AllowAutoRedirect);

			return result.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, null, x => x.ExceptionInfo(ex));
		}
	}
}


public class ConfigureRPOClientAdapterConfig : Microsoft.Extensions.Options.IConfigureOptions<RPOClientAdapterConfig>
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	public ConfigureRPOClientAdapterConfig(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	public void Configure(RPOClientAdapterConfig options)
	{
		using (var scope = _serviceScopeFactory.CreateScope())
		{
			var provider = scope.ServiceProvider;
		}
	}
}
