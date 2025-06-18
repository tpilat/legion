using Legion.ADF.ESB.ComponentsModel;
using Legion.ADF.ESB.Components.Model;
using Legion.ADF.ESB.ComponentsModel.PostgreSQL;
using Legion.ADF.ESB.ServiceBus.Extensions;
using Legion.ADF.ESB.ServiceBus.PostgreSQL;
using Legion.ADF.ESB.TestConsole.Commands;
using Legion.EntityFrameworkCore;
using Legion.Model.Audit;
using Legion.Extensions;
using Legion.MessageBus;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.ESB.TestConsole;

internal class Test
{
	public const string ADF_ESB_STORE_ID = "ADF_ESB";

	public static async Task RunAsync(CancellationToken cancellationToken = default)
	{
		var services = new ServiceCollection();

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		services.AddLogging();

		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			 .AddJsonFile("appsettings.json");

		var configuration = builder.Build();
		services.AddSingleton<IConfiguration>(configuration);

		services.AddADFEngerpriseServiceBus(c =>
		{
			c.ConfigurePostgreSQL(services);
		});

		services.AddInMemoryMessageBus([typeof(Test).Assembly]);

		var sp = services.BuildServiceProvider();

		var descs = sp.GetAllServiceDescriptors();

		await using var asyncServiceScope = sp.CreateAsyncScope();

		//consoleapp: simulate on start validation in hosted apps (winservice, asp.net, ..)
		var startupOptionsValidator = asyncServiceScope.ServiceProvider.GetRequiredService<IStartupValidator>();
		startupOptionsValidator.Validate();

		var connectionProviderFactory = asyncServiceScope.ServiceProvider.GetRequiredService<IEFConnectionProviderFactory>();

		await using var connectionProvider = connectionProviderFactory.CreateWithNewTransactionByStoreId<IEFConnectionProviderFactory>(
			asyncServiceScope.ServiceProvider,
			ADF_ESB_STORE_ID,
			new TransactionsController(),
			null);

		IAuditEntryStore? auditUnitOfWork = null;
		IComponentsUnitOfWork entityUnitOfWork = new ComponentsUnitOfWork(connectionProvider, auditUnitOfWork);
		IComponentsQueryUnitOfWork queryUnitOfWork = new ComponentsQueryUnitOfWork(connectionProvider, auditUnitOfWork);

		var scopeContext = ScopeContext.Create("TEST");

		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<Test>();
		scopeContext.AppendTraceFrameWithLogger(logger, true);

		var jobTypes = await entityUnitOfWork.JobTypeRepository
			.AsQueryable(scopeContext.CreateNew())
			.ToListAsync(cancellationToken);

		var commitResult = await connectionProvider.TransactionsController!.CommitAllAsync(scopeContext, false, cancellationToken);
		commitResult.ThrowIfError(scopeContext, null/*//TODO*/, true);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var mbusRes = await messageBus.SendAsync(scopeContext, new TestCommand(IdMessage: Guid.NewGuid()));


		var ok1 = Components.Model.AdapterStatus.Disabled == Components.Model.AdapterStatusEnum.Disabled.ToGuid();

		var ok2 = (AdapterStatusEnum)Components.Model.AdapterStatus.Disabled_NewObject == Components.Model.AdapterStatusEnum.Disabled;
	}
}
