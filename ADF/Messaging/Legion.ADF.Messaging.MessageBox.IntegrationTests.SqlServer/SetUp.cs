using Legion.ADF.Logs.Extensions;
using Legion.ADF.Messaging.MessageBox.IntegrationTests.Messages;
using Legion.Database.SqlServer;
using Legion.Extensions;
using Legion.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text;
using Testcontainers.MsSql;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[SetUpFixture]
public class SetUp
{
	private static readonly AsyncLock _servicesLock = new();
	private static MsSqlContainer _sqlServerContainer;
	private static IConfiguration _configuration;
	private static IServiceScopeFactory? _scopeFactory;

	public static string ConncetionString => "Server=127.0.0.1;Database=legion_adf_msg;User Id=sa;Password=Password1.;TrustServerCertificate=True;MultipleActiveResultSets=true";
	//public static string ConncetionString => $"{_sqlServerContainer.GetConnectionString()};Encrypt=False";

	[OneTimeSetUp]
	public async Task RunBeforeAnyTests()
	{
		//ConfigureSqlServerContainer();
		//await _sqlServerContainer.StartAsync();

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
			.ConfigureSqlServer();

		var scopeContext = ScopeContext.Create("TEST ScopeContext");

		services.AddADFMessaging(_configuration)
			.AddMessageBox(messageBox =>
			{
				messageBox
					.AddMessageType<TestMessage>(scopeContext)
					.AddMessageType(scopeContext, "myString", "myString", "myString")
					.AddQueue<Events.Test1QueueMessageReceivedEvent>(
						scopeContext,
						queueName: "TestQueue",
						factory: msg => new Events.Test1QueueMessageReceivedEvent(msg),
						timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
						isSequentialFIFO: false,
						messagesBatchCount: 1,
						maxDegreeOfParallelism: 1,
						maxMessageProcessingRetryCount: 5,
						messageTypeNamespace: "myString")
					.AddQueue<Events.Test2QueueMessageReceivedEvent>(
						scopeContext,
						queueName: "FakeQueue",
						factory: msg => new Events.Test2QueueMessageReceivedEvent(msg),
						timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
						isSequentialFIFO: false,
						messagesBatchCount: 1,
						maxDegreeOfParallelism: 1,
						maxMessageProcessingRetryCount: 5,
							messageTypeNamespace: "myString")
						.AddQueue<Events.Test3QueueMessageReceivedEvent>(
							scopeContext,
							queueName: "NoHandlerQueue",
							factory: msg => new Events.Test3QueueMessageReceivedEvent(msg),
							timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
							isSequentialFIFO: false,
							messagesBatchCount: 1,
							maxDegreeOfParallelism: 1,
							maxMessageProcessingRetryCount: 5,
							messageTypeNamespace: "myString")
						.AddTopic(
							scopeContext,
							topicName: "TestTopic",
							timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
							isSequentialFIFO: false,
							messagesBatchCount: 1,
							maxDegreeOfParallelism: 1,
							maxMessageProcessingRetryCount: 5,
							configureSubscriptions: sub => sub.RegisterSubscription<Events.Test1TopicSubscriptionMessageReceivedEvent>(
								"TestTopicSubscription",
								factory: msg => new Events.Test1TopicSubscriptionMessageReceivedEvent(msg),
								idJob: null,
								idOrchestration: null))
						.AddTopic(
							scopeContext,
							topicName: "FakeTopic",
							timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
							isSequentialFIFO: false,
							messagesBatchCount: 1,
							maxDegreeOfParallelism: 1,
							maxMessageProcessingRetryCount: 5,
							configureSubscriptions: sub => sub.RegisterSubscription<Events.Test2TopicSubscriptionMessageReceivedEvent>(
								"FakeTopicSubscription",
								factory: msg => new Events.Test2TopicSubscriptionMessageReceivedEvent(msg),
								idJob: null,
								idOrchestration: null))
						.AddTopic(
							scopeContext,
							topicName: "NoHandlerTopic",
							timeoutForMessageProcessing: TimeSpan.FromMicroseconds(1),
							isSequentialFIFO: false,
							messagesBatchCount: 1,
							maxDegreeOfParallelism: 1,
							maxMessageProcessingRetryCount: 5,
							configureSubscriptions: sub => sub.RegisterSubscription<Events.Test3TopicSubscriptionMessageReceivedEvent>(
								"NoHandlerTopicSubscription",
								factory: msg => new Events.Test3TopicSubscriptionMessageReceivedEvent(msg),
								idJob: null,
								idOrchestration: null))
						.ConfigureMessageBoxSqlServer();
				});

		services.AddInMemoryMessageBus([typeof(SqlServer.TableInfoProvider).Assembly, typeof(SetUp).Assembly]);

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
		if (_sqlServerContainer != null)
		{
			await _sqlServerContainer.StopAsync();
			await _sqlServerContainer.DisposeAsync();
		}
	}

	private static void ConfigureSqlServerContainer()
	{
		_sqlServerContainer = new MsSqlBuilder()
			.WithImage("mcr.microsoft.com/mssql/server:2022-RTM-ubuntu-20.04") // Optional: Specify a custom SQL Server image version
			.WithPassword("yourStrong(!)Password") // Set the MSSQL password
			.WithPortBinding(1433, true) // Optional: Bind the internal port to a random free port on the host (or use a fixed one)
			.WithCleanUp(true) // Optional: Automatically remove the container after the tests finish
			.WithName("testcontainers-mssql") // Optional: Set a custom container name
			.WithStartupCallback(DeployDB)
			.Build();

		static async Task DeployDB(MsSqlContainer container, CancellationToken cancellationToken)
		{
			var encoding = new UTF8Encoding(false);
			var baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string? executeResult;

			await Task.Delay(10000);

			var connectionString = $"{container.GetConnectionString()};Encrypt=False";

			var connection = new SqlConnection(connectionString);
			connection.Open();

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "schemas.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "tables.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "views.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);

			//var permission = File.ReadAllText(Path.Combine(baseDir, "DB", "permissions.sql"), encoding);
			//permission = permission.Replace("#TargetDbUsername#", "sa");

			//executeResult = SqlScript.Execute(connection, permission, true);
			//if (!string.IsNullOrWhiteSpace(executeResult))
			//	Throw.InvalidOperationException(executeResult);

			executeResult = SqlScript.Execute(connection, File.ReadAllText(Path.Combine(baseDir, "DB", "data_initial.sql"), encoding), true);
			if (!string.IsNullOrWhiteSpace(executeResult))
				Throw.InvalidOperationException(executeResult);
		}
	}
}
