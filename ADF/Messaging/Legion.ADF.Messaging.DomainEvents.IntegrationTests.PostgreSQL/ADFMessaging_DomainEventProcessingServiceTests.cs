using Legion.ADF.Messaging.DomainEvents.IntegrationTests.PostgreSQL;
using Legion.ADF.Messaging.DomainEvents.Services;
using Legion.ADF.Messaging.DomainEvents.Services.Internal;
using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.MessageBus;
using Legion.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.DomainEvents.IntegrationTests;

[Category("ADFMessaging DomainEventProcessingService tests")]
public class DomainEventProcessingServiceTests : TestBase
{
	protected override void SetupTestInternal()
	{
		var options = new DomainEventProcessingServiceOptions
		{
			IdleTimeout = TimeSpan.FromSeconds(1),
			MessagesBatchCount = 10,
			NextProcessingTimeout = TimeSpan.FromSeconds(30),
			MaxRetryCount = 3,
			MaxDegreeOfParallelism = 2
		};
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessDomainEvents_WithNoHandler()
	{
		//reset CACHED blocked domain event namespaces
		new ObjectWrapper<DomainEventStore>(null)["_blockedDomainEventNamespaces"] = null;

		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<DomainEventProcessingService>();
		var options = sp.GetRequiredService<IOptions<DomainEventProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.MessagesBatchCount = 10;
		options.Value.NextProcessingTimeout = TimeSpan.FromSeconds(30);
		options.Value.MaxRetryCount = 3;
		options.Value.MaxDegreeOfParallelism = 2;
		options.Value.DisableMultiProcessingLog = true;

		var service = new DomainEventProcessingService(options, sp, logger);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var domainEventsStore = sp.GetRequiredService<DomainEventStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var domainEvent = new TestDomainEventWithNoHandler("MyMessageContent with no handler");

		var createResult = await domainEventsStore.SaveDomainEventAsync(
			scopeContext,
			domainEvent,
			propertiesJson: null,
			"TestCase",
			"001",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != null);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var domainEventQuery = new Queries.DomainEvent.GetDomainEventByIdQuery(domainEvent.Id, false, false, true, null);
		var savedDomainEventResult = await messageBus.SendAsync(scopeContext, domainEventQuery);

		Assert.That(!savedDomainEventResult.HasError && savedDomainEventResult.Data != null && savedDomainEventResult.Data.IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.NoHandler);

		var logsQuery = new Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEventQuery(domainEvent.Id, false, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 2);
		Assert.That(logs![0].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Processing);
		Assert.That(logs![1].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.NoHandler);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessDomainEvents_WithHandler()
	{
		//reset CACHED blocked domain event namespaces
		new ObjectWrapper<DomainEventStore>(null)["_blockedDomainEventNamespaces"] = null;

		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<DomainEventProcessingService>();
		var options = sp.GetRequiredService<IOptions<DomainEventProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.MessagesBatchCount = 10;
		options.Value.NextProcessingTimeout = TimeSpan.FromSeconds(30);
		options.Value.MaxRetryCount = 3;
		options.Value.MaxDegreeOfParallelism = 2;
		options.Value.DisableMultiProcessingLog = true;

		var service = new DomainEventProcessingService(options, sp, logger);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var domainEventsStore = sp.GetRequiredService<DomainEventStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var domainEvent = new TestDomainEvent("MyMessageContent");

		var createResult = await domainEventsStore.SaveDomainEventAsync(
			scopeContext,
			domainEvent,
			propertiesJson: null,
			"TestCase",
			"001",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != null);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var domainEventQuery = new Queries.DomainEvent.GetDomainEventByIdQuery(domainEvent.Id, false, false, true, null);
		var savedDomainEventResult = await messageBus.SendAsync(scopeContext, domainEventQuery);

		Assert.That(!savedDomainEventResult.HasError && savedDomainEventResult.Data != null && savedDomainEventResult.Data.IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Processed);

		var logsQuery = new Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEventQuery(domainEvent.Id, false, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 2);
		Assert.That(logs![0].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Processing);
		Assert.That(logs![1].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Processed);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessDomainEvents_ExceedingMaxRetryCount()
	{
		//reset CACHED blocked domain event namespaces
		new ObjectWrapper<DomainEventStore>(null)["_blockedDomainEventNamespaces"] = null;

		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<DomainEventProcessingService>();
		var options = sp.GetRequiredService<IOptions<DomainEventProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromMilliseconds(1);
		options.Value.MessagesBatchCount = 10;
		options.Value.NextProcessingTimeout = TimeSpan.FromMilliseconds(1);
		options.Value.MaxRetryCount = 3;
		options.Value.MaxDegreeOfParallelism = 2;
		options.Value.DisableMultiProcessingLog = true;

		var service = new DomainEventProcessingService(options, sp, logger);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var domainEventsStore = sp.GetRequiredService<DomainEventStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var domainEvent = new FailureTestDomainEvent("MyMessageContent with error handler");

		var createResult = await domainEventsStore.SaveDomainEventAsync(
			scopeContext,
			domainEvent,
			propertiesJson: null,
			"TestCase",
			"001",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != null);

		//await using (var uow = CreateDomainEventsUnitOfWork(scopeContext, sp))
		//{
		//	var de = await uow.DomainEventRepository
		//		.AsQueryable(scopeContext)
		//		.Where(x => x.IdDomainEvent == createResult.Data)
		//		.FirstOrDefaultAsync(cancellationToken: default);

		//	Assert.That(de, Is.Not.EqualTo(null));

		//	de.SetProcessing(scopeContext, options.Value.ProcessingTimeoutInSeconds, disableProcessingLog: false);

		//	var errorMessage = new ErrorMessageBuilder(scopeContext, new ErrorCode("ERR_CODE", "ERR_MSG", "ERR_DESC")).Build();

		//	for (int i = 0; i < options.Value.MaxRetryCount - 1; i++)
		//		de.SetFailed(scopeContext, options.Value.MaxRetryCount, nextProcessingDelayInSeconds: 1, disableProcessingLog: false, errorMessage);

		//	await uow.SaveAsync(scopeContext, cancellationToken: default);
		//}

		//await Task.Delay(1000);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var domainEventQuery = new Queries.DomainEvent.GetDomainEventByIdQuery(domainEvent.Id, false, false, true, null);
		var savedDomainEventResult = await messageBus.SendAsync(scopeContext, domainEventQuery);

		Assert.That(!savedDomainEventResult.HasError && savedDomainEventResult.Data != null && savedDomainEventResult.Data.IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Suspended);

		var logsQuery = new Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEventQuery(domainEvent.Id, false, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 4);
		Assert.That(logs![0].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Processing);
		Assert.That(logs![1].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Failed);
		Assert.That(logs![2].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Failed);
		Assert.That(logs![3].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Suspended);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessDomainEvents_ForBlockedEvent()
	{
		//reset CACHED blocked domain event namespaces
		new ObjectWrapper<DomainEventStore>(null)["_blockedDomainEventNamespaces"] = null;

		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<DomainEventProcessingService>();
		var options = sp.GetRequiredService<IOptions<DomainEventProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.MessagesBatchCount = 10;
		options.Value.NextProcessingTimeout = TimeSpan.FromSeconds(30);
		options.Value.MaxRetryCount = 3;
		options.Value.MaxDegreeOfParallelism = 2;
		options.Value.DisableMultiProcessingLog = true;

		var service = new DomainEventProcessingService(options, sp, logger);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var domainEventsStore = sp.GetRequiredService<DomainEventStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var domainEvent = new TestDomainEvent("Blocked MyMessageContent");

		var createResult = await domainEventsStore.SaveDomainEventAsync(
			scopeContext,
			domainEvent,
			propertiesJson: null,
			"TestCase",
			"001",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != null);

		//INSERT BlockedDomainEventType
		var connectionProviderFactory = sp.GetRequiredService<IConnectionProviderFactory>();
		await using (var connectionProvider = connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			sp,
			storeId: null,
			transactionIsolationLevel: null,
			allowLocking: false,
			createAuditEntryStore: false))
		{
			var domainEventsUowResult = connectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(scopeContext);

			if (domainEventsUowResult.HasError)
				domainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

			var uow = domainEventsUowResult.Data!;
			var blockedDEResult = Model.BlockedDomainEventType.Create(scopeContext, domainEvent.Namespace);

			Assert.That(!blockedDEResult.HasError && blockedDEResult.Data != null);

			uow.BlockedDomainEventTypeRepository.Add(
				scopeContext,
				blockedDEResult.Data!);

			var blockDEResult = await uow.SaveAsync(scopeContext, cancellationToken: default);

			Assert.That(!blockDEResult.HasError && blockDEResult.Data == 1);

			await connectionProvider.CommitAllAsync(scopeContext, cancellationToken: default);
		}

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var domainEventQuery = new Queries.DomainEvent.GetDomainEventByIdQuery(domainEvent.Id, false, false, true, null);
		var savedDomainEventResult = await messageBus.SendAsync(scopeContext, domainEventQuery);

		Assert.That(!savedDomainEventResult.HasError && savedDomainEventResult.Data != null && savedDomainEventResult.Data.IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Blocked);

		var logsQuery = new Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEventQuery(domainEvent.Id, false, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 2);
		Assert.That(logs![0].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Processing);
		Assert.That(logs![1].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Blocked);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessDomainEvents_ForInvalidEvent()
	{
		//reset CACHED blocked domain event namespaces
		new ObjectWrapper<DomainEventStore>(null)["_blockedDomainEventNamespaces"] = null;

		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<DomainEventProcessingService>();
		var options = sp.GetRequiredService<IOptions<DomainEventProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.MessagesBatchCount = 10;
		options.Value.NextProcessingTimeout = TimeSpan.FromSeconds(30);
		options.Value.MaxRetryCount = 3;
		options.Value.MaxDegreeOfParallelism = 2;
		options.Value.DisableMultiProcessingLog = true;

		var service = new DomainEventProcessingService(options, sp, logger);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var domainEventsStore = sp.GetRequiredService<DomainEventStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var domainEvent = new TestDomainEvent("Blocked MyMessageContent");

		var createResult = await domainEventsStore.SaveDomainEventAsync(
			scopeContext,
			domainEvent,
			propertiesJson: null,
			"TestCase",
			"001",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != null);

		//Update Namespace
		var connectionProviderFactory = sp.GetRequiredService<IConnectionProviderFactory>();
		await using (var connectionProvider = connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			sp,
			storeId: null,
			transactionIsolationLevel: null,
			allowLocking: false,
			createAuditEntryStore: false))
		{
			var domainEventsUowResult = connectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(scopeContext);

			if (domainEventsUowResult.HasError)
				domainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

			var uow = domainEventsUowResult.Data!;
			await uow.ExecuteSqlInterpolatedAsync(scopeContext, $"UPDATE devt.\"DomainEvent\" SET \"Namespace\" = 'XXX' WHERE \"IdDomainEvent\" = {createResult.Data}", cancellationToken: default);
		}

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var domainEventQuery = new Queries.DomainEvent.GetDomainEventByIdQuery(domainEvent.Id, false, false, true, null);
		var savedDomainEventResult = await messageBus.SendAsync(scopeContext, domainEventQuery);

		Assert.That(!savedDomainEventResult.HasError && savedDomainEventResult.Data != null && savedDomainEventResult.Data.IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Suspended);

		var logsQuery = new Queries.DomainEventProcessingLog.GetAllDomainEventProcessingLogsByIdDomainEventQuery(domainEvent.Id, false, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 2);
		Assert.That(logs![0].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Processing);
		Assert.That(logs![1].IdDomainEventProcessingStatus == Model.DomainEventProcessingStatus.Suspended);

		await service.StopAsync(cancellationTokenSource.Token);
	}
}
