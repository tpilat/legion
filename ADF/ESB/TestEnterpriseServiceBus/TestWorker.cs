using Legion;
using Legion.ADF.ESB;
using Legion.ADF.ESB.Components;
using Legion.ADF.ESB.Components.PostgreSQL;
using Legion.Logging;
using Legion.MessageBus;
using Legion.Model.Repositories;
using Legion.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestEnterpriseServiceBus.Adapters.SocPoist;

namespace TestEnterpriseServiceBus;

public class TestWorker : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<TestWorker> _logger;

	public TestWorker(ILogger<TestWorker> logger, IServiceProvider serviceProvider)
	{
		Throw.IfArgumentNull(serviceProvider);
		Throw.IfArgumentNull(logger);

		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var lgr = LoggerMessage.Define<ILogMessage>(LogLevel.Information, new EventId(-1, "LM"), "{@message}");

		while (!cancellationToken.IsCancellationRequested)
		{
			await using var asyncServiceScope = _serviceProvider.CreateAsyncScope();
			var sp = asyncServiceScope.ServiceProvider;

			var scopeContext = ScopeContext.Create("TestWorker", targetStoreId: null);

			if (_logger.IsEnabled(LogLevel.Information))
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			}

			var transactionsController = new TransactionsController();

			await using var invocationContext =
				new InvocationContextBuilder(scopeContext)
				.Initialize(sp, transactionsController, false)
				.Build();

			var componentsUowResult = invocationContext.CreateUnitOfWork<IComponentsUnitOfWork, ConnectionStringProvider>();
			var componentsUoW = componentsUowResult.Data!;

			var dbAdapters =
				await componentsUoW.AdapterRepository
					.AsQueryable(scopeContext)
					.ToListAsync(cancellationToken);

			var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

			//var mbusResSync = messageBus.Send(
			//	invocationContext,
			//	new Adapters.SocPoist.Messages.SocPoistRequest()
			//	//,CallOptions.Create(Legion.Policy.RetryOptionsFactory.Create(3, TimeSpan.FromSeconds(1))/*, TimeSpan.FromMilliseconds(10)*/)
			//	//,CallOptions.CreateFireAndForget()
			//	//,CallOptions.Create(TimeSpan.FromSeconds(1))
			//	);

			var socPoistResponse = await messageBus.SendAsync(
				invocationContext,
				new Adapters.SocPoist.Messages.SocPoistRequest(),
				//CallOptions.Create(Legion.Policy.RetryOptionsFactory.Create(3, TimeSpan.FromSeconds(5)), TimeSpan.FromMilliseconds(10)),
				//CallOptions.CreateFireAndForget(),
				//CallOptions.Create(TimeSpan.FromSeconds(1)),
				cancellationToken);

			await using var asyncServiceScope2 = _serviceProvider.CreateAsyncScope();
			var sp2 = asyncServiceScope2.ServiceProvider;

			var transactionsController2 = new TransactionsController();
			await using var invocationContext2 =
				new InvocationContextBuilder(scopeContext)
				.Initialize(sp2, transactionsController2, false)
				.Build();

			var socPoistResponse2 = await messageBus.SendAsync(
				invocationContext2,
				new Adapters.SocPoist.Messages.SocPoistRequest(),
				//CallOptions.Create(Legion.Policy.RetryOptionsFactory.Create(3, TimeSpan.FromSeconds(5)), TimeSpan.FromMilliseconds(10)),
				//CallOptions.CreateFireAndForget(),
				//CallOptions.Create(TimeSpan.FromSeconds(1)),
				cancellationToken);

			var rpoResponse = await messageBus.SendAsync(
				invocationContext,
				new Adapters.RPO.Messages.RPORequest(),
				//CallOptions.Create(Legion.Policy.RetryOptionsFactory.Create(3, TimeSpan.FromSeconds(5)), TimeSpan.FromMilliseconds(10)),
				//CallOptions.CreateFireAndForget(),
				//CallOptions.Create(TimeSpan.FromSeconds(1)),
				cancellationToken);

			var componentsQueryUowResult = invocationContext.CreateQueryUnitOfWork<IComponentsQueryUnitOfWork, ConnectionStringProvider>();
			var componentsQueryUoW = componentsQueryUowResult.Data!;

			var jobs = await componentsQueryUoW.VwJobRepository
				.GetVwJobById(new Legion.ADF.ESB.Components.Queries.VwJob.GetVwJobByIdQuery(new Guid("00000002-0000-0000-0000-000000000000"), null))
				.ToResultAsync(invocationContext, cancellationToken);

			await transactionsController.CommitAllAsync(invocationContext, false, cancellationToken);

			_logger.LogWarningMessage(scopeContext, null, x => x.InternalMessage($"Adapters count {dbAdapters.Count}"));

			await Task.Delay(2000, cancellationToken);
		}
	}
}
