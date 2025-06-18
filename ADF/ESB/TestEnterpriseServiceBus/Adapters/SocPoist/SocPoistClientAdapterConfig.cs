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
using TestEnterpriseServiceBus.Adapters.SocPoist.Http;

namespace TestEnterpriseServiceBus.Adapters.SocPoist;

public class SocPoistClientAdapterConfig : ESBAdapterConfig, IESBAdapterConfig, IServiceCollectionOptionsBuilder
{
	private const string BASE_CONFIG_PATH = "TestEnterpriseServiceBusConfig";
	private const string DEFAULT_HREF_PREFIX = "/api/idsp/download/";

	public SocPoistHttpClientOptions SocPoistHttpClientOptions { get; private set; }
	public string HrefPrefix { get; set; }

	public SocPoistClientAdapterConfig()
	{
		SocPoistHttpClientOptions = new SocPoistHttpClientOptions();
		HrefPrefix = DEFAULT_HREF_PREFIX;
	}

	public class Validator : ValidatorBase<SocPoistClientAdapterConfig>
	{
		public Validator() { }
		public Validator(IObjectPath objectPath) : base(objectPath) { }

		public override void SetDefaultRuels(ValidatorBuilder<SocPoistClientAdapterConfig> builder)
			=> RulesBuilder(builder);

		public static void RulesBuilder(ValidatorBuilder<SocPoistClientAdapterConfig> builder)
		{
			builder?
				.ForProperty(x => x.SocPoistHttpClientOptions, v => v.NotNull())
				.ForNavigation(
					x => x.SocPoistHttpClientOptions,
					SocPoistHttpClientOptions.SocPoistHttpClientOptionsValidator.RulesBuilder)
				.ForProperty(x => x.HrefPrefix, v => v.NotDefaultOrWhiteSpace())
			;
		}
	}

	public static IServiceCollection ConfigureOptions(IServiceCollection services)
	{
		Throw.IfArgumentNull(services);

		services
			.AddAndConfigureOptions<SocPoistClientAdapterConfig>(
				b => b.BindConfiguration($"{BASE_CONFIG_PATH}:{nameof(SocPoistClientAdapterConfig)}"),
				(sp, o) =>
				{
					if (string.IsNullOrWhiteSpace(o.HrefPrefix))
						o.HrefPrefix = DEFAULT_HREF_PREFIX;

					if (ESBInitializer.ConfigsInitializationStatus != ESBInitializationStatus.Finished)
					{
						o.SetDefaultOptions();

						//var path = $"{BASE_CONFIG_PATH}:{nameof(SocPoistClientAdapterConfig)}";
						//var json = JsonSerializerHelper.Serialize(o);
						//var dict = Legion.Configuration.JsonConfigurationParser_Newtonsoft.Parse(json, path, null, false);
						//var mngr = new Legion.ADF.Config.ConfigModel.PostgreSQL.Configuration.DBConfigurationManager(
						//		opt => opt.UseNpgsql(string.Format("Host=host.docker.internal;Database=legion_adf_esb;Port=35432;Username=legion_adf_esb_usr;Password=legion_adf_esb_pwd", System.Environment.GetEnvironmentVariable("PGPASSWORD"))),
						//		sp.GetRequiredService<ILogger<Legion.ADF.Config.ConfigModel.PostgreSQL.ConfigDbContext>>());

						//mngr.SaveDataByPath(ScopeContext.Create(nameof(SocPoistClientAdapterConfig)), path, dict, false, true);

						return;
					}

					var transactionsController = new TransactionsController();

					var scopeContext = ScopeContext.Create(nameof(SocPoistClientAdapterConfig));
					using var invocationContext =
						new InvocationContextBuilder(scopeContext)
						.Initialize(sp, transactionsController, false)
						.Build();

					var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
					var logger = loggerFactory.CreateLogger<SocPoistClientAdapterConfig>();

					var componentsUowResult = invocationContext.CreateUnitOfWork<IComponentsUnitOfWork, Legion.ADF.ESB.ConnectionStringProvider>();
					var componentsUoW = componentsUowResult.Data!;

					var dbProperties =
						componentsUoW.AdapterRepository
							.AsQueryable(scopeContext)
							.Where(x => x.IdAdapter == SocPoistClientAdapter.ADAPTER_ID)
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

	public override SocPoistClientAdapterConfig GetDefaultOptions()
	{
		var result = new SocPoistClientAdapterConfig();
		result.SetDefaultOptions();
		return result;
	}

	public override void SetDefaultOptions()
	{
		HrefPrefix = DEFAULT_HREF_PREFIX;
		MinLogLevel = Microsoft.Extensions.Logging.LogLevel.Warning;
		SocPoistHttpClientOptions = new SocPoistHttpClientOptions
		{
			ClientName = "SocPoistHttpClient",
			SourceSystemName = "SocPoistHttpClient",
			UserAgent = "SocPoistHttpClient",
			Version = new Version(1, 0, 0, 0),
			BaseAddress = "https://www.socpoist.sk",
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
			var savedOptions = JsonSerializerHelper.Deserialize<SocPoistClientAdapterConfig>(savedProperties);
			if (savedOptions == null)
				return result.WithInvalidOperationException(scopeContext, null, x => x.InternalMessage($"{nameof(savedOptions)} == null"));

			MinLogLevel = ValueGetterHelper.GetNewValueIfSet(MinLogLevel, savedOptions.MinLogLevel);
			HrefPrefix = ValueGetterHelper.GetNewValueIfSet(HrefPrefix, savedOptions.HrefPrefix);

			if (savedOptions.SocPoistHttpClientOptions == null)
				return result.Build();

			SocPoistHttpClientOptions ??= new();

			SocPoistHttpClientOptions.ClientName = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.ClientName, savedOptions.SocPoistHttpClientOptions.ClientName);
			SocPoistHttpClientOptions.SourceSystemName = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.SourceSystemName, savedOptions.SocPoistHttpClientOptions.SourceSystemName);
			SocPoistHttpClientOptions.UserAgent = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.UserAgent, savedOptions.SocPoistHttpClientOptions.UserAgent);
			SocPoistHttpClientOptions.Version = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.Version, savedOptions.SocPoistHttpClientOptions.Version);
			SocPoistHttpClientOptions.BaseAddress = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.BaseAddress, savedOptions.SocPoistHttpClientOptions.BaseAddress);
			SocPoistHttpClientOptions.DefaultTimeoutInSeconds = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.DefaultTimeoutInSeconds, savedOptions.SocPoistHttpClientOptions.DefaultTimeoutInSeconds);
			SocPoistHttpClientOptions.LogRequest = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.LogRequest, savedOptions.SocPoistHttpClientOptions.LogRequest);
			SocPoistHttpClientOptions.LogRequestPayload = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.LogRequestPayload, savedOptions.SocPoistHttpClientOptions.LogRequestPayload);
			SocPoistHttpClientOptions.LogResponse = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.LogResponse, savedOptions.SocPoistHttpClientOptions.LogResponse);
			SocPoistHttpClientOptions.LogResponsePayload = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.LogResponsePayload, savedOptions.SocPoistHttpClientOptions.LogResponsePayload);
			SocPoistHttpClientOptions.TrustToAllServerCertificates = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.TrustToAllServerCertificates, savedOptions.SocPoistHttpClientOptions.TrustToAllServerCertificates);
			SocPoistHttpClientOptions.UsesCookieContainerToStoreServerCookies = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.UsesCookieContainerToStoreServerCookies, savedOptions.SocPoistHttpClientOptions.UsesCookieContainerToStoreServerCookies);
			SocPoistHttpClientOptions.StaticQueryStrings = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.StaticQueryStrings, savedOptions.SocPoistHttpClientOptions.StaticQueryStrings);
			SocPoistHttpClientOptions.ForceStaticQueryStrings = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.ForceStaticQueryStrings, savedOptions.SocPoistHttpClientOptions.ForceStaticQueryStrings);
			SocPoistHttpClientOptions.StaticHeaders = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.StaticHeaders, savedOptions.SocPoistHttpClientOptions.StaticHeaders);
			SocPoistHttpClientOptions.StaticHeaderCollections = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.StaticHeaderCollections, savedOptions.SocPoistHttpClientOptions.StaticHeaderCollections);
			SocPoistHttpClientOptions.StaticCookies = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.StaticCookies, savedOptions.SocPoistHttpClientOptions.StaticCookies);
			SocPoistHttpClientOptions.StaticFormData = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.StaticFormData, savedOptions.SocPoistHttpClientOptions.StaticFormData);
			SocPoistHttpClientOptions.AutomaticDecompression = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.AutomaticDecompression, savedOptions.SocPoistHttpClientOptions.AutomaticDecompression);
			SocPoistHttpClientOptions.WebProxySettings = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.WebProxySettings, savedOptions.SocPoistHttpClientOptions.WebProxySettings);
			SocPoistHttpClientOptions.CheckCertificateRevocationList = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.CheckCertificateRevocationList, savedOptions.SocPoistHttpClientOptions.CheckCertificateRevocationList);
			SocPoistHttpClientOptions.UseDefaultCredentials = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.UseDefaultCredentials, savedOptions.SocPoistHttpClientOptions.UseDefaultCredentials);
			SocPoistHttpClientOptions.SendAuthorizationHeaderInRequest = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.SendAuthorizationHeaderInRequest, savedOptions.SocPoistHttpClientOptions.SendAuthorizationHeaderInRequest);
			SocPoistHttpClientOptions.SslProtocols = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.SslProtocols, savedOptions.SocPoistHttpClientOptions.SslProtocols);
			SocPoistHttpClientOptions.MaxResponseHeadersLength = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.MaxResponseHeadersLength, savedOptions.SocPoistHttpClientOptions.MaxResponseHeadersLength);
			SocPoistHttpClientOptions.MaxRequestContentBufferSize = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.MaxRequestContentBufferSize, savedOptions.SocPoistHttpClientOptions.MaxRequestContentBufferSize);
			SocPoistHttpClientOptions.MaxConnectionsPerServer = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.MaxConnectionsPerServer, savedOptions.SocPoistHttpClientOptions.MaxConnectionsPerServer);
			SocPoistHttpClientOptions.MaxAutomaticRedirections = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.MaxAutomaticRedirections, savedOptions.SocPoistHttpClientOptions.MaxAutomaticRedirections);
			SocPoistHttpClientOptions.AllowAutoRedirect = ValueGetterHelper.GetNewValueIfSet(SocPoistHttpClientOptions.AllowAutoRedirect, savedOptions.SocPoistHttpClientOptions.AllowAutoRedirect);

			return result.Build();
		}
		catch (Exception ex)
		{
			return result.WithInvalidOperationException(scopeContext, null, x => x.ExceptionInfo(ex));
		}
	}
}
