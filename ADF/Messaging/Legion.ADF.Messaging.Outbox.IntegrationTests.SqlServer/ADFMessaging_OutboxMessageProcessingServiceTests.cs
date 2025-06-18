using Legion.ADF.Messaging.Outbox.IntegrationTests.Messages;
using Legion.ADF.Messaging.Outbox.Services;
using Legion.ADF.Messaging.Outbox.Services.Internal;
using Legion.ADF.Messaging.Settings;
using Legion.MessageBus;
using Legion.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests;

[Category("ADFMessaging OutboxMessageProcessingService tests")]
public class OutboxMessageProcessingServiceTests : TestBase
{
	protected override void SetupTestInternal()
	{
		var options = new OutboxMessageProcessingServiceOptions
		{
			IdleTimeout = TimeSpan.FromSeconds(1),
			LogLevel = LogLevel.Trace
		};
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessOutbox_WithNoHandler()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<OutboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<OutboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new OutboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var outboxMessage = new TestMessage("MyMessageContent with no handler");

		var outboxMessageDto = DTOs.OutboxMessageDto.CreateJsonMessage(
			outboxMessage,
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await outboxStore.CreateOutboxMessageAsync(
			scopeContext,
			outboxMessageDto,
			"NoHandlerQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var outboxMessageQuery = new Queries.OutboxMessage.GetOutboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedOutboxMessageResult = await messageBus.SendAsync(scopeContext, outboxMessageQuery);

		Assert.That(!savedOutboxMessageResult.HasError && savedOutboxMessageResult.Data != null && savedOutboxMessageResult.Data.IdOutboxMessageStatus == Model.OutboxMessageStatus.NoHandler);

		var logsQuery = new Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdOutboxMessageStatus == Model.OutboxMessageStatus.Created);
		Assert.That(logs![1].IdOutboxMessageStatus == Model.OutboxMessageStatus.Processing);
		Assert.That(logs![2].IdOutboxMessageStatus == Model.OutboxMessageStatus.NoHandler);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessOutbox_WithHandler()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<OutboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<OutboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new OutboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var outboxMessage = new TestMessage("MyMessageContent");

		var outboxMessageDto = DTOs.OutboxMessageDto.CreateJsonMessage(
			outboxMessage,
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await outboxStore.CreateOutboxMessageAsync(
			scopeContext,
			outboxMessageDto,
			"TestQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var outboxMessageQuery = new Queries.OutboxMessage.GetOutboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedOutboxMessageResult = await messageBus.SendAsync(scopeContext, outboxMessageQuery);

		Assert.That(!savedOutboxMessageResult.HasError && savedOutboxMessageResult.Data != null && savedOutboxMessageResult.Data.IdOutboxMessageStatus == Model.OutboxMessageStatus.Processed);

		var logsQuery = new Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdOutboxMessageStatus == Model.OutboxMessageStatus.Created);
		Assert.That(logs![1].IdOutboxMessageStatus == Model.OutboxMessageStatus.Processing);
		Assert.That(logs![2].IdOutboxMessageStatus == Model.OutboxMessageStatus.Processed);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessOutbox_ExceedingMaxRetryCount()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<OutboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<OutboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new OutboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var outboxMessage = new TestMessage("MyMessageContent with error handler");

		var outboxMessageDto = DTOs.OutboxMessageDto.CreateStringMessage(
			"myString",
			"MyMessageContent with error handler",
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await outboxStore.CreateOutboxMessageAsync(
			scopeContext,
			outboxMessageDto,
			"FakeQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		//await using (var uow = CreateOutboxUnitOfWork(scopeContext, sp))
		//{
		//	var de = await uow.OutboxMessageRepository
		//		.AsQueryable(scopeContext)
		//		.Where(x => x.IdOutboxMessage == createResult.Data)
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
		var outboxMessageQuery = new Queries.OutboxMessage.GetOutboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedOutboxMessageResult = await messageBus.SendAsync(scopeContext, outboxMessageQuery);

		Assert.That(!savedOutboxMessageResult.HasError && savedOutboxMessageResult.Data != null && savedOutboxMessageResult.Data.IdOutboxMessageStatus == Model.OutboxMessageStatus.Suspended);

		var logsQuery = new Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 7);
		Assert.That(logs![0].IdOutboxMessageStatus == Model.OutboxMessageStatus.Created);
		Assert.That(logs![1].IdOutboxMessageStatus == Model.OutboxMessageStatus.Processing);
		Assert.That(logs![2].IdOutboxMessageStatus == Model.OutboxMessageStatus.Failed);
		Assert.That(logs![3].IdOutboxMessageStatus == Model.OutboxMessageStatus.Failed);
		Assert.That(logs![4].IdOutboxMessageStatus == Model.OutboxMessageStatus.Failed);
		Assert.That(logs![5].IdOutboxMessageStatus == Model.OutboxMessageStatus.Failed);
		Assert.That(logs![6].IdOutboxMessageStatus == Model.OutboxMessageStatus.Suspended);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessOutbox_ForBlockedOutboxMessage()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<OutboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<OutboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new OutboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var outboxMessageDto = DTOs.OutboxMessageDto.CreateStringMessage(
			"myString",
			"Blocked MyMessageContent",
			 Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await outboxStore.CreateOutboxMessageAsync(
			scopeContext,
			outboxMessageDto,
			"TestQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		var createBlocketResult = await outboxStore.AddBlockedMessageTypesAsync(scopeContext, new List<string> { outboxMessageDto.MessageTypeNamespace }, false, cancellationToken: default);
		Assert.That(!createBlocketResult.HasError);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var outboxMessageQuery = new Queries.OutboxMessage.GetOutboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedOutboxMessageResult = await messageBus.SendAsync(scopeContext, outboxMessageQuery);

		Assert.That(!savedOutboxMessageResult.HasError && savedOutboxMessageResult.Data != null && savedOutboxMessageResult.Data.IdOutboxMessageStatus == Model.OutboxMessageStatus.Blocked);

		var logsQuery = new Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdOutboxMessageStatus == Model.OutboxMessageStatus.Created);
		Assert.That(logs![1].IdOutboxMessageStatus == Model.OutboxMessageStatus.Processing);
		Assert.That(logs![2].IdOutboxMessageStatus == Model.OutboxMessageStatus.Blocked);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessOutbox_ForInvalidOutboxMessage()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<OutboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<OutboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new OutboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var outboxMessageDto = DTOs.OutboxMessageDto.CreateStringMessage(
			"myString",
			"Blocked MyMessageContent",
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await outboxStore.CreateOutboxMessageAsync(
			scopeContext,
			outboxMessageDto,
			"TestQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		var outboxMessageTypeRegistry = sp.GetRequiredService<OutboxMessageTypeRegistry>();
		new ObjectWrapper<OutboxMessageTypeRegistry>(outboxMessageTypeRegistry).SetValue("_reseted", false);
		var newOutboxMessageTypeResult = Model.OutboxMessageType.Create(scopeContext, "XXX", "XXX", "XXX");
		newOutboxMessageTypeResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		outboxMessageTypeRegistry.ResetOutboxMessageTypes(scopeContext, new List<Model.OutboxMessageType> { newOutboxMessageTypeResult.Data! });

		////Update Namespace
		//var connectionProviderFactory = sp.GetRequiredService<IConnectionProviderFactory>();
		//await using (var connectionProvider = connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
		//	sp,
		//	storeId: null,
		//	transactionIsolationLevel: null,
		//	allowLocking: false,
		//	createAuditEntryStore: false))
		//{
		//	var outboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IOutboxUnitOfWork>(scopeContext);

		//	if (outboxUowResult.HasError)
		//		outboxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.OutboxUnitOfWorkException.InvalidUoW, true);

		//	var uow = outboxUowResult.Data!;
		//	await uow.ExecuteSqlInterpolatedAsync(scopeContext, $"UPDATE outbox.\"OutboxMessage\" SET \"Namespace\" = 'XXX' WHERE \"IdOutboxMessage\" = {createResult.Data}", cancellationToken: default);
		//}

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var outboxMessageQuery = new Queries.OutboxMessage.GetOutboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedOutboxMessageResult = await messageBus.SendAsync(scopeContext, outboxMessageQuery);

		Assert.That(!savedOutboxMessageResult.HasError && savedOutboxMessageResult.Data != null && savedOutboxMessageResult.Data.IdOutboxMessageStatus == Model.OutboxMessageStatus.UnknownType);

		var logsQuery = new Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdOutboxMessageStatus == Model.OutboxMessageStatus.Created);
		Assert.That(logs![1].IdOutboxMessageStatus == Model.OutboxMessageStatus.Processing);
		Assert.That(logs![2].IdOutboxMessageStatus == Model.OutboxMessageStatus.UnknownType);

		await service.StopAsync(cancellationTokenSource.Token);
	}
}
