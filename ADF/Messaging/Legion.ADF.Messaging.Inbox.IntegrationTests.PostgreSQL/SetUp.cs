using Legion.ADF.Logs.Extensions;
using Legion.ADF.Messaging.Inbox.IntegrationTests.Messages;
using Legion.Database.PostgreSQL;
using Legion.Extensions;
using Legion.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using System.Text;
using Testcontainers.PostgreSql;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests;

[SetUpFixture]
public class SetUp
{
	private static readonly AsyncLock _servicesLock = new();
	private static PostgreSqlContainer _postgreSqlContainer;
	private static IConfiguration _configuration;
	private static IServiceScopeFactory? _scopeFactory;

	public static string ConncetionString => "Host=localhost;Database=legion_adf_msg;Port=5432;Username=postgres;Password=postgres;Timeout=120;CommandTimeout=120;MaxPoolSize=20;ApplicationName=Legion.ADF.Messaging.Inbox.IntegrationTests";
	//public static string ConncetionString => _postgreSqlContainer.GetConnectionString();

	[OneTimeSetUp]
	public async Task RunBeforeAnyTests()
	{
		//ConfigurePostgreSqlContainer();
		//await _postgreSqlContainer.StartAsync();

		_configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			//.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			//.AddEnvironmentVariables()
			//.AddCommandLine([] /*args*/)
			.AddConfigurationData(new Dictionary<string, string?>
				{
					{
						"ADFMessaging:DBSettings:DbConnectionSettings:ADF_Messaging:ConnectionString",
						ConncetionString
					},
					{
						"ADFLogs:DBSettings:DbConnectionSettings:ADF_AUDIT:ConnectionString",
						ConncetionString
					},
					{
						"ADFLogs:LoggerSettings:UseBatchWriter",
						"true"
					},
					{
						"ADFLogs:LoggerSettings:LogMessageMinLogLevel",
						"Trace"
					},
					{
						"ADFLogs:LoggerSettings:UnstructuredLogMinLogLevel",
						"Trace"
					},
					{
						"ADFLogs:BatchLogMessageStoreOptions:Period",
						"00:00:00.020"
					},
					{
						"ADFLogs:BatchUnstructuredLogStoreOptions:Period",
						"00:00:00.020"
					}
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

			services.AddADFLogs(_configuration)
				.ConfigurePostgreSQL();

			var scopeContext = ScopeContext.Create("TEST ScopeContext");

			services.AddADFMessaging(_configuration)
				.AddInbox(inbox =>
				{
					inbox
						.AddInboxMessageType<TestMessage>(scopeContext)
						.AddInboxMessageType(scopeContext, "myString", "myString", "myString")
						.AddInboxQueue<Events.Test1QueueInboxMessageReceivedEvent>(
							scopeContext,
							queueName: "TestQueue",
							factory: msg => new Events.Test1QueueInboxMessageReceivedEvent(msg),
							timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
							isSequentialFIFO: false,
							messagesBatchCount: 1,
							maxDegreeOfParallelism: 1,
							maxMessageProcessingRetryCount: 5,
							messageTypeNamespace: "myString")
						.AddInboxQueue<Events.Test2QueueInboxMessageReceivedEvent>(
							scopeContext,
							queueName: "FakeQueue",
							factory: msg => new Events.Test2QueueInboxMessageReceivedEvent(msg),
							timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
							isSequentialFIFO: false,
							messagesBatchCount: 1,
							maxDegreeOfParallelism: 1,
							maxMessageProcessingRetryCount: 5,
							messageTypeNamespace: "myString")
						.AddInboxQueue<Events.Test3QueueInboxMessageReceivedEvent>(
							scopeContext,
							queueName: "NoHandlerQueue",
							factory: msg => new Events.Test3QueueInboxMessageReceivedEvent(msg),
							timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
							isSequentialFIFO: false,
							messagesBatchCount: 1,
							maxDegreeOfParallelism: 1,
							maxMessageProcessingRetryCount: 5,
							messageTypeNamespace: "myString")
						.ConfigureInboxPostgreSQL();
				});

			services.AddInMemoryMessageBus([typeof(PostgreSQL.TableInfoProvider).Assembly, typeof(SetUp).Assembly]);

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
					correlationId: GlobalContext.Instance.NewGuid(),
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
		if (_postgreSqlContainer != null)
		{
			await _postgreSqlContainer.StopAsync();
			await _postgreSqlContainer.DisposeAsync();
		}
	}

	public const string usr = "msgusr";
	private static void ConfigurePostgreSqlContainer()
	{
		_postgreSqlContainer = new PostgreSqlBuilder()
			//.WithImage("postgres:16.3")
			.WithImage("postgres:14.12")
			.WithDatabase("legion_adf_msg")
			.WithUsername(usr)
			.WithPassword("msgpwd")
			.WithStartupCallback(DeployDB)
			.Build();

		static async Task DeployDB(PostgreSqlContainer container, CancellationToken cancellationToken)
		{
			var encoding = new UTF8Encoding(false);
			var baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string? executeResult;

			await Task.Delay(500);

			var connection = new NpgsqlConnection(ConncetionString);
			connection.Open();

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "configuredb.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "schemas.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "tables.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "views.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			var permission = File.ReadAllText(Path.Combine(baseDir, "DB", "permissions.sql"), encoding);
			permission = permission.Replace("#TargetDbUsername#", usr);

			executeResult = SqlScript.Execute(connection, permission, true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "data_initial.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);
		}
	}
}
