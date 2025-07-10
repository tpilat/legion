using Legion.ADF.Cache.RestApi.Client;
using Legion.Extensions;
using Legion.Threading;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Legion.ADF.Cache.IntegrationTests;

[SetUpFixture]
public class SetUp
{
	private static readonly AsyncLock _servicesLock = new();
	private static IConfiguration _configuration;
	private static IServiceScopeFactory? _scopeFactory;

	public static string ConncetionString => "Host=localhost;Database=legion_adf_cache;Port=5432;Username=postgres;Password=postgres;Timeout=120;CommandTimeout=120;MaxPoolSize=20;ApplicationName=Legion.ADF.Cache.IntegrationTests";
	//public static string ConncetionString => _postgreSqlContainer.GetConnectionString();

	public static HttpClient HttpClient { get; private set; }

	[OneTimeSetUp]
	public async Task RunBeforeAnyTests()
	{
		var webApplicationFactory = new WebApplicationFactory<RestApi.Program>();
		HttpClient = webApplicationFactory.CreateClient(
			new WebApplicationFactoryClientOptions
			{
				BaseAddress = new Uri("https://localhost")
			});

		_configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			//.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			//.AddEnvironmentVariables()
			//.AddCommandLine([] /*args*/)
			.AddConfigurationData(new Dictionary<string, string?>
				{
					//{
					//	"CacheRestApi:BaseAddress",
					//	"https://localhost:7139/api/v1"
					//}
				})
			.Build();
	}

	private static async Task<IServiceScopeFactory> GetScopeFactoryAsync()
	{
		if (_scopeFactory != null)
			return _scopeFactory;

		using (await _servicesLock.LockAsync())
		{
			if (_scopeFactory != null)
				return _scopeFactory;

			var services = new ServiceCollection();
			services.TryAddSingleton(_configuration);

			services.AddCacheRestApiClient("CacheRestApi");

			//services.AddInMemoryMessageBus([typeof(RestApi.Client.CacheRestApiClient).Assembly, typeof(SetUp).Assembly]);

			services.AddApplicationEntryScopeContext(sp =>
			{
				Microsoft.Extensions.Logging.ILogger? logger = null;
				var loggerFactory = sp.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
				if (loggerFactory != null)
					logger = Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger<SetUp>(loggerFactory);

				var scopeContext = ScopeContext.Create(
					"TEST_BASE",
					removePreviousSameMethodFrame: true,
					previousScopeContext: null,
					correlationId: Guid.NewGuid(),
					principal: null,
					idUser: null,
					businessProcess: null,
					component: "WEB",
					tenantIdentifier: null,
					externalCorrelationId: null,
					customCorrelationId: null,
					logger: logger,
					cultureInfo: new System.Globalization.CultureInfo("sk"),
					requestMetadata: null,
					cancellationToken: null);

				return scopeContext;
			});

			var serivceProvider = services.BuildServiceProvider();
			_scopeFactory = serivceProvider.GetRequiredService<IServiceScopeFactory>();

			await using var scope = _scopeFactory.CreateAsyncScope();
			await scope.ServiceProvider.RunStartupTasksAsync();
		}

		return _scopeFactory;
	}

	public static async Task<IServiceProvider> CreateScopedServiceProviderAsync()
		=> (await GetScopeFactoryAsync()).CreateScope().ServiceProvider;

	public static void ClearScopedServiceProvider()
		=> _scopeFactory = null;

	[OneTimeTearDown]
	public async Task RunAfterAnyTests()
	{
		if (HttpClient != null)
		{
			HttpClient.Dispose();
		}
	}
}
