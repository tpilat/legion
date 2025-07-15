using Legion.ADF.Messaging.MessageBox.IntegrationTests.Messages;
using Legion.ADF.Messaging.MessageBox.Services;
using Legion.ADF.Messaging.MessageBox.Services.Internal;
using Legion.ADF.Messaging.Settings;
using Legion.MessageBus;
using Legion.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[Category("ADFMessaging SubscribedMessageProcessingService tests")]
public class SubscribedMessageProcessingServiceTests : TestBase
{
	protected override void SetupTestInternal()
	{
		var options = new MessageBoxMessageProcessingServiceOptions
		{
			IdleTimeout = TimeSpan.FromSeconds(1),
			LogLevel = LogLevel.Trace
		};
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessMessageBox_WithNoHandler()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<SubscribedMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<MessageBoxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new SubscribedMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var message = new TestMessage("MyMessageContent with no handler");

		var messageDto = DTOs.MessageBoxMessageDto.CreateJsonMessage(
			message,
			GlobalContext.Instance.NewGuid().ToString(),
			"my-publicher",
			 GlobalContext.Instance.NewGuid().ToString());

		var createResult = await messageBoxStore.CreateMessageAsync(
			scopeContext,
			messageDto,
			queueName: null,
			topicName: "NoHandlerTopic",
			subscriptionName: "NoHandlerTopicSubscription",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var subscribedMessagesQuery = new Queries.SubscribedMessage.GetSubscribedMessagesByIdMessageQuery(createResult.Data, false, true, null);
		var savedSubscribedMessagesResult = await messageBus.SendAsync(scopeContext, subscribedMessagesQuery);

		Assert.That(!savedSubscribedMessagesResult.HasError && savedSubscribedMessagesResult.Data != null && savedSubscribedMessagesResult.Data.Count == 1 && savedSubscribedMessagesResult.Data[0].IdMessageProcessingStatus == Model.MessageProcessingStatus.NoHandler);

		var logsQuery = new Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Created);
		Assert.That(logs![1].IdMessageProcessingStatus == Model.MessageProcessingStatus.Processing);
		Assert.That(logs![2].IdMessageProcessingStatus == Model.MessageProcessingStatus.NoHandler);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessMessageBox_WithHandler()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<SubscribedMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<MessageBoxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new SubscribedMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var message = new TestMessage("MyMessageContent");

		var messageDto = DTOs.MessageBoxMessageDto.CreateJsonMessage(
			message,
			GlobalContext.Instance.NewGuid().ToString(),
			"my-publicher",
			 GlobalContext.Instance.NewGuid().ToString());

		var createResult = await messageBoxStore.CreateMessageAsync(
			scopeContext,
			messageDto,
			queueName: null,
			topicName: "TestTopic",
			subscriptionName: "TestTopicSubscription",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var subscribedMessagesQuery = new Queries.SubscribedMessage.GetSubscribedMessagesByIdMessageQuery(createResult.Data, false, true, null);
		var savedSubscribedMessagesResult = await messageBus.SendAsync(scopeContext, subscribedMessagesQuery);

		Assert.That((bool)(!savedSubscribedMessagesResult.HasError && savedSubscribedMessagesResult.Data != null && savedSubscribedMessagesResult.Data.Count == 1 && savedSubscribedMessagesResult.Data[0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Processed));

		var logsQuery = new Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Created);
		Assert.That(logs![1].IdMessageProcessingStatus == Model.MessageProcessingStatus.Processing);
		Assert.That(logs![2].IdMessageProcessingStatus == Model.MessageProcessingStatus.Processed);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldProcessMessageBox_ExceedingMaxRetryCount()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<SubscribedMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<MessageBoxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new SubscribedMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var message = new TestMessage("MyMessageContent with error handler");

		var messageDto = DTOs.MessageBoxMessageDto.CreateStringMessage(
			"myString",
			"MyMessageContent with error handler",
			GlobalContext.Instance.NewGuid().ToString(),
			"my-publicher",
			 GlobalContext.Instance.NewGuid().ToString());

		var createResult = await messageBoxStore.CreateMessageAsync(
			scopeContext,
			messageDto,
			queueName: null,
			topicName: "FakeTopic",
			subscriptionName: "FakeTopicSubscription",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		//await using (var uow = CreateMessageBoxUnitOfWork(scopeContext, sp))
		//{
		//	var de = await uow.MessageRepository
		//		.AsQueryable(scopeContext)
		//		.Where(x => x.IdMessage == createResult.Data)
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
		var subscribedMessagesQuery = new Queries.SubscribedMessage.GetSubscribedMessagesByIdMessageQuery(createResult.Data, false, true, null);
		var savedSubscribedMessagesResult = await messageBus.SendAsync(scopeContext, subscribedMessagesQuery);

		Assert.That((bool)(!savedSubscribedMessagesResult.HasError && savedSubscribedMessagesResult.Data != null && savedSubscribedMessagesResult.Data.Count == 1 && savedSubscribedMessagesResult.Data[0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Suspended));

		var logsQuery = new Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 7);
		Assert.That(logs![0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Created);
		Assert.That(logs![1].IdMessageProcessingStatus == Model.MessageProcessingStatus.Processing);
		Assert.That(logs![2].IdMessageProcessingStatus == Model.MessageProcessingStatus.Failed);
		Assert.That(logs![3].IdMessageProcessingStatus == Model.MessageProcessingStatus.Failed);
		Assert.That(logs![4].IdMessageProcessingStatus == Model.MessageProcessingStatus.Failed);
		Assert.That(logs![5].IdMessageProcessingStatus == Model.MessageProcessingStatus.Failed);
		Assert.That(logs![6].IdMessageProcessingStatus == Model.MessageProcessingStatus.Suspended);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessMessageBox_ForBlockedMessage()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<SubscribedMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<MessageBoxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new SubscribedMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var messageDto = DTOs.MessageBoxMessageDto.CreateStringMessage(
			"myString",
			"Blocked MyMessageContent",
			 GlobalContext.Instance.NewGuid().ToString(),
			"my-publicher",
			 GlobalContext.Instance.NewGuid().ToString());

		var createResult = await messageBoxStore.CreateMessageAsync(
			scopeContext,
			messageDto,
			queueName: null,
			topicName: "TestTopic",
			subscriptionName: "TestTopicSubscription",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		var createBlocketResult = await messageBoxStore.AddBlockedMessageTypesAsync(scopeContext, new List<string> { messageDto.MessageTypeNamespace }, false, cancellationToken: default);
		Assert.That(!createBlocketResult.HasError);

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var subscribedMessagesQuery = new Queries.SubscribedMessage.GetSubscribedMessagesByIdMessageQuery(createResult.Data, false, true, null);
		var savedSubscribedMessagesResult = await messageBus.SendAsync(scopeContext, subscribedMessagesQuery);

		Assert.That((bool)(!savedSubscribedMessagesResult.HasError && savedSubscribedMessagesResult.Data != null && savedSubscribedMessagesResult.Data.Count == 1 && savedSubscribedMessagesResult.Data[0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Blocked));

		var logsQuery = new Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Created);
		Assert.That(logs![1].IdMessageProcessingStatus == Model.MessageProcessingStatus.Processing);
		Assert.That(logs![2].IdMessageProcessingStatus == Model.MessageProcessingStatus.Blocked);

		await service.StopAsync(cancellationTokenSource.Token);
	}

	[Test]
	public async Task ExecuteAsync_ShouldNotProcessMessageBox_ForInvalidMessage()
	{
		var serviceTimeoutInSeconds = 2;
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<SubscribedMessageProcessingService>();
		var options = sp.GetRequiredService<IOptions<MessageBoxMessageProcessingServiceOptions>>();

		options.Value.IdleTimeout = TimeSpan.FromSeconds(1);
		options.Value.DisableMultiProcessingLog = true;
		options.Value.LogLevel = LogLevel.Trace;

		var service = new SubscribedMessageProcessingService(options, sp);

		var cancellationTokenSource = new CancellationTokenSource();
		cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(serviceTimeoutInSeconds));

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var messageDto = DTOs.MessageBoxMessageDto.CreateStringMessage(
			"myString",
			"Blocked MyMessageContent",
			GlobalContext.Instance.NewGuid().ToString(),
			"my-publicher",
			 GlobalContext.Instance.NewGuid().ToString());

		var createResult = await messageBoxStore.CreateMessageAsync(
			scopeContext,
			messageDto,
			queueName: null,
			topicName: "TestTopic",
			subscriptionName: "TestTopicSubscription",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != Guid.Empty);

		var messageTypeRegistry = sp.GetRequiredService<MessageTypeRegistry>();
		new ObjectWrapper<MessageTypeRegistry>(messageTypeRegistry).SetValue("_reseted", false);
		var newMessageTypeResult = Model.MessageType.Create(scopeContext, "XXX", "XXX", "XXX");
		newMessageTypeResult.ThrowIfErrorOrNullData(scopeContext, null, true);
		messageTypeRegistry.ResetMessageTypes(scopeContext, new List<Model.MessageType> { newMessageTypeResult.Data! });

		////Update Namespace
		//var connectionProviderFactory = sp.GetRequiredService<IConnectionProviderFactory>();
		//await using (var connectionProvider = connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
		//	sp,
		//	storeId: null,
		//	transactionIsolationLevel: null,
		//	allowLocking: false,
		//	createAuditEntryStore: false))
		//{
		//	var messageBoxUowResult = connectionProvider.UnitOfWorkProvider.Create<IMessageBoxUnitOfWork>(scopeContext);

		//	if (messageBoxUowResult.HasError)
		//		messageBoxUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.MessageBoxUnitOfWorkException.InvalidUoW, true);

		//	var uow = messageBoxUowResult.Data!;
		//	await uow.ExecuteSqlInterpolatedAsync(scopeContext, $"UPDATE messageBox.\"Message\" SET \"Namespace\" = 'XXX' WHERE \"IdMessage\" = {createResult.Data}", cancellationToken: default);
		//}

		await service.StartAsync(cancellationTokenSource.Token);

		await Task.Delay(serviceTimeoutInSeconds * 1000);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var subscribedMessagesQuery = new Queries.SubscribedMessage.GetSubscribedMessagesByIdMessageQuery(createResult.Data, false, true, null);
		var savedSubscribedMessagesResult = await messageBus.SendAsync(scopeContext, subscribedMessagesQuery);

		Assert.That((bool)(!savedSubscribedMessagesResult.HasError && savedSubscribedMessagesResult.Data != null && savedSubscribedMessagesResult.Data.Count == 1 && savedSubscribedMessagesResult.Data[0].IdMessageProcessingStatus == Model.MessageProcessingStatus.UnknownType));

		var logsQuery = new Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery(createResult.Data, false, true, true, null);
		var savedLogsResult = await messageBus.SendAsync(scopeContext, logsQuery);

		var logs = savedLogsResult.Data?.OrderBy(depl => depl.CreatedUtc).ToList();
		Assert.That(!savedLogsResult.HasError && logs != null && logs.Count == 3);
		Assert.That(logs![0].IdMessageProcessingStatus == Model.MessageProcessingStatus.Created);
		Assert.That(logs![1].IdMessageProcessingStatus == Model.MessageProcessingStatus.Processing);
		Assert.That(logs![2].IdMessageProcessingStatus == Model.MessageProcessingStatus.UnknownType);

		await service.StopAsync(cancellationTokenSource.Token);
	}
}
