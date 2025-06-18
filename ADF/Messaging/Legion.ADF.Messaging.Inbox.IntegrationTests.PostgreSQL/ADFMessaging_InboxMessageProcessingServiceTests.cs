using Legion.ADF.Messaging.Inbox.IntegrationTests.Messages;
using Legion.ADF.Messaging.Inbox.Services;
using Legion.ADF.Messaging.Inbox.Services.Internal;
using Legion.ADF.Messaging.Settings;
using Legion.MessageBus;
using Legion.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests;

[Category("ADFMessaging InboxMessageProcessingService tests")]
public class InboxMessageProcessingServiceTests : TestBase
{
	protected override void SetupTestInternal()
	{
		var options = new InboxMessageProcessingServiceOptions
		{
			IdleTimeout = TimeSpan.FromSeconds(1),
			LogLevel = LogLevel.Trace
		};
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessInbox_WithNoHandler()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<InboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<InboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new InboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var inboxMessage = new TestMessage("MyMessageContent with no handler");

		var inboxMessageDto = DTOs.InboxMessageDto.CreateJsonMessage(
			inboxMessage,
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await inboxStore.CreateInboxMessageAsync(
			scopeContext,
			inboxMessageDto,
			"NoHandlerQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var inboxMessageQuery = new Queries.InboxMessage.GetInboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedInboxMessageResult = await messageBus.SendAsync(scopeContext, inboxMessageQuery);

		Assert.That(!savedInboxMessageResult.HasError && savedInboxMessageResult.Data != null && savedInboxMessageResult.Data.IdInboxMessageStatus == Model.InboxMessageStatus.NoHandler);

		var logsQuery = new Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdInboxMessageStatus == Model.InboxMessageStatus.Created);
		Assert.That(logs![1].IdInboxMessageStatus == Model.InboxMessageStatus.Processing);
		Assert.That(logs![2].IdInboxMessageStatus == Model.InboxMessageStatus.NoHandler);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessInbox_WithHandler()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<InboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<InboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new InboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var inboxMessage = new TestMessage("MyMessageContent");

		var inboxMessageDto = DTOs.InboxMessageDto.CreateJsonMessage(
			inboxMessage,
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await inboxStore.CreateInboxMessageAsync(
			scopeContext,
			inboxMessageDto,
			"TestQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var inboxMessageQuery = new Queries.InboxMessage.GetInboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedInboxMessageResult = await messageBus.SendAsync(scopeContext, inboxMessageQuery);

		Assert.That(!savedInboxMessageResult.HasError && savedInboxMessageResult.Data != null && savedInboxMessageResult.Data.IdInboxMessageStatus == Model.InboxMessageStatus.Processed);

		var logsQuery = new Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdInboxMessageStatus == Model.InboxMessageStatus.Created);
		Assert.That(logs![1].IdInboxMessageStatus == Model.InboxMessageStatus.Processing);
		Assert.That(logs![2].IdInboxMessageStatus == Model.InboxMessageStatus.Processed);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessInbox_ExceedingMaxRetryCount()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<InboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<InboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new InboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var inboxMessage = new TestMessage("MyMessageContent with error handler");

		var inboxMessageDto = DTOs.InboxMessageDto.CreateStringMessage(
			"myString",
			"MyMessageContent with error handler",
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await inboxStore.CreateInboxMessageAsync(
			scopeContext,
			inboxMessageDto,
			"FakeQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		//await using (var uow = CreateInboxUnitOfWork(scopeContext, sp))
		//{
		//	var de = await uow.InboxMessageRepository
		//		.AsQueryable(scopeContext)
		//		.Where(x => x.IdInboxMessage == createResult.Data)
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
		var inboxMessageQuery = new Queries.InboxMessage.GetInboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedInboxMessageResult = await messageBus.SendAsync(scopeContext, inboxMessageQuery);

		Assert.That(!savedInboxMessageResult.HasError && savedInboxMessageResult.Data != null && savedInboxMessageResult.Data.IdInboxMessageStatus == Model.InboxMessageStatus.Suspended);

		var logsQuery = new Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 7);
		Assert.That(logs![0].IdInboxMessageStatus == Model.InboxMessageStatus.Created);
		Assert.That(logs![1].IdInboxMessageStatus == Model.InboxMessageStatus.Processing);
		Assert.That(logs![2].IdInboxMessageStatus == Model.InboxMessageStatus.Failed);
		Assert.That(logs![3].IdInboxMessageStatus == Model.InboxMessageStatus.Failed);
		Assert.That(logs![4].IdInboxMessageStatus == Model.InboxMessageStatus.Failed);
		Assert.That(logs![5].IdInboxMessageStatus == Model.InboxMessageStatus.Failed);
		Assert.That(logs![6].IdInboxMessageStatus == Model.InboxMessageStatus.Suspended);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessInbox_ForBlockedInboxMessage()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<InboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<InboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new InboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var inboxMessageDto = DTOs.InboxMessageDto.CreateStringMessage(
			"myString",
			"Blocked MyMessageContent",
			 Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await inboxStore.CreateInboxMessageAsync(
			scopeContext,
			inboxMessageDto,
			"TestQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		var createBlocketResult = await inboxStore.AddBlockedMessageTypesAsync(scopeContext, new List<string> { inboxMessageDto.MessageTypeNamespace }, false, cancellationToken: default);
		Assert.That(!createBlocketResult.HasError);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var inboxMessageQuery = new Queries.InboxMessage.GetInboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedInboxMessageResult = await messageBus.SendAsync(scopeContext, inboxMessageQuery);

		Assert.That(!savedInboxMessageResult.HasError && savedInboxMessageResult.Data != null && savedInboxMessageResult.Data.IdInboxMessageStatus == Model.InboxMessageStatus.Blocked);

		var logsQuery = new Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdInboxMessageStatus == Model.InboxMessageStatus.Created);
		Assert.That(logs![1].IdInboxMessageStatus == Model.InboxMessageStatus.Processing);
		Assert.That(logs![2].IdInboxMessageStatus == Model.InboxMessageStatus.Blocked);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessInbox_ForInvalidInboxMessage()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<InboxMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<InboxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new InboxMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var inboxMessageDto = DTOs.InboxMessageDto.CreateStringMessage(
			"myString",
			"Blocked MyMessageContent",
			Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await inboxStore.CreateInboxMessageAsync(
			scopeContext,
			inboxMessageDto,
			"TestQueue",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		var inboxMessageTypeRegistry = sp.GetRequiredService<InboxMessageTypeRegistry>();
		new ObjectWrapper<InboxMessageTypeRegistry>(inboxMessageTypeRegistry).SetValue("_reseted", false);
		var newInboxMessageTypeResult = Model.InboxMessageType.Create(scopeContext, "XXX", "XXX", "XXX");
		newInboxMessageTypeResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		inboxMessageTypeRegistry.ResetInboxMessageTypes(scopeContext, new List<Model.InboxMessageType> { newInboxMessageTypeResult.Data! });

		////Update Namespace
		//var connectionProviderFactory = sp.GetRequiredService<IConnectionProviderFactory>();
		//await using (var connectionProvider = connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
		//	sp,
		//	storeId: null,
		//	transactionIsolationLevel: null,
		//	allowLocking: false,
		//	createAuditEntryStore: false))
		//{
		//	var inboxUowResult = connectionProvider.UnitOfWorkProvider.Create<IInboxUnitOfWork>(scopeContext);

		//	if (inboxUowResult.HasError)
		//		inboxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.InboxUnitOfWorkException.InvalidUoW, true);

		//	var uow = inboxUowResult.Data!;
		//	await uow.ExecuteSqlInterpolatedAsync(scopeContext, $"UPDATE inbox.\"InboxMessage\" SET \"Namespace\" = 'XXX' WHERE \"IdInboxMessage\" = {createResult.Data}", cancellationToken: default);
		//}

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var inboxMessageQuery = new Queries.InboxMessage.GetInboxMessageByIdQuery(createResult.Data, false, false, true, null);
		var savedInboxMessageResult = await messageBus.SendAsync(scopeContext, inboxMessageQuery);

		Assert.That(!savedInboxMessageResult.HasError && savedInboxMessageResult.Data != null && savedInboxMessageResult.Data.IdInboxMessageStatus == Model.InboxMessageStatus.UnknownType);

		var logsQuery = new Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdInboxMessageStatus == Model.InboxMessageStatus.Created);
		Assert.That(logs![1].IdInboxMessageStatus == Model.InboxMessageStatus.Processing);
		Assert.That(logs![2].IdInboxMessageStatus == Model.InboxMessageStatus.UnknownType);

		await service.StopAsync(cancellationTokenSource.Token);
	}
}
