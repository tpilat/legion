using Legion.ADF.Messaging.Outbox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests;

[NUnit.Framework.Category("ADFMessaging OutboxMessage tests")]
public class ADFMessaging_OutboxMessageTests : TestBase
{
	internal static async Task<(DTOs.OutboxMessageDto OutboxMessageDto, Guid IdOutboxMessage)> CreateOutboxMessageAsync(
		IScopeContext scopeContext,
		string @namespace,
		string message,
		string queueName,
		IOutboxStore outboxStore)
	{
		scopeContext = ScopeContext.Create(scopeContext);
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		var outboxMessageDto = DTOs.OutboxMessageDto.CreateStringMessage(
			@namespace,
			message,
			 Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await outboxStore.CreateOutboxMessageAsync(
			scopeContext,
			outboxMessageDto,
			queueName,
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		return (outboxMessageDto, createResult.Data);
	}

	[Test]
	public async Task OutboxMessage_ShouldCreateOutboxMessage()
	{
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var tmpResult = await ADFMessaging_OutboxMessageTests.CreateOutboxMessageAsync(
			scopeContext,
			"myString",
			"fakeTxt",
			"FakeQueue",
			outboxStore);

		var queueName = "TestQueue";

		var messageText = "TestText";
		var createResult = await ADFMessaging_OutboxMessageTests.CreateOutboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			outboxStore);

		createResult = await ADFMessaging_OutboxMessageTests.CreateOutboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			outboxStore);

		var queueQuery = new Queries.OutboxQueue.GetAllOutboxQueuesQuery(false, true, true, true, null);
		var queueResult = await messageBus.SendAsync(scopeContext, queueQuery);

		Assert.That(!queueResult.HasError && queueResult.Data != null && queueResult.Data?.Count == 3);

		await using var uow = CreateOutboxUnitOfWork(scopeContext, sp);
		var queue = await uow.OutboxQueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var query = new Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueueQuery(queue.IdOutboxQueue, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.Count(x => x.IdMessageType == queueResult.Data!.First(x => x.IdOutboxQueue == queue.IdOutboxQueue).IdMessageType) == 2);

		var outboxQueueMessages = await outboxStore.GetOutboxQueueMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwOutboxQueueMessages>()
				.SortBy(x => x.OutboxQueueName, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!outboxQueueMessages.HasError
			&& outboxQueueMessages.Data != null
			&& outboxQueueMessages.Data?.TotalCount == 3
			&& outboxQueueMessages.Data.Data.First(q => q.IdOutboxQueue == queue.IdOutboxQueue).CreatedMessageCount == 2);

		var outboxQueueOutboxMessages = await outboxStore.GetAllOutboxMessagesAsync(
			scopeContext,
			queue.IdOutboxQueue,
			new SortDescriptorBuilder<Model.VwOutboxMessage>()
				.SortBy(x => x.CreatedUtc, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!outboxQueueOutboxMessages.HasError
			&& outboxQueueOutboxMessages.Data != null
			&& outboxQueueOutboxMessages.Data?.TotalCount == 2
			&& outboxQueueOutboxMessages.Data.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdOutboxQueue == queue.IdOutboxQueue).IdMessageType);

		var outboxMessage = await outboxStore.GetOutboxMessageAsync(scopeContext, createResult.IdOutboxMessage, true, cancellationToken: default);

		Assert.That(
			!outboxMessage.HasError
			&& outboxMessage.Data != null
			&& outboxMessage.Data.IdMessageType == queueResult.Data!.First(x => x.IdOutboxQueue == queue.IdOutboxQueue).IdMessageType);

		var outboxMessageContent = await outboxStore.GetOutboxMessageContentAsync(scopeContext, createResult.IdOutboxMessage, true, cancellationToken: default);

		Assert.That(
			!outboxMessageContent.HasError
			&& outboxMessageContent.Data != null
			&& outboxMessageContent.Data.StringContent == messageText);
	}

	[Test]
	public async Task OutboxMessage_ShouldArchivateOutboxMessage()
	{
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var tmpResult = await ADFMessaging_OutboxMessageTests.CreateOutboxMessageAsync(
			scopeContext,
			"myString",
			"fakeTxt",
			"FakeQueue",
			outboxStore);

		var queueName = "TestQueue";

		var messageText = "TestText";
		var createResult = await ADFMessaging_OutboxMessageTests.CreateOutboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			outboxStore);

		createResult = await ADFMessaging_OutboxMessageTests.CreateOutboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			outboxStore);

		var queueQuery = new Queries.OutboxQueue.GetAllOutboxQueuesQuery(false, true, true, true, null);
		var queueResult = await messageBus.SendAsync(scopeContext, queueQuery);

		Assert.That(!queueResult.HasError && queueResult.Data != null && queueResult.Data?.Count == 3);

		await using var uow = CreateOutboxUnitOfWork(scopeContext, sp);
		var queue = await uow.OutboxQueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var messageQuery = new Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueueQuery(queue.IdOutboxQueue, true, true, true, null);
		var messageResult = await messageBus.SendAsync(scopeContext, messageQuery);

		Assert.That(!messageResult.HasError && messageResult.Data != null && messageResult.Data.Count(x => x.IdMessageType == queueResult.Data!.First(x => x.IdOutboxQueue == queue.IdOutboxQueue).IdMessageType) == 2);

		var archiveQuery = new Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery(queue.IdOutboxQueue, true, true, true, null);
		var archiveResult = await messageBus.SendAsync(scopeContext, archiveQuery);

		Assert.That(!archiveResult.HasError && archiveResult.Data != null && archiveResult.Data.Count == 0);

		var createArchiveResult = await outboxStore.ArchivateOutboxMessageAsync(
			scopeContext,
			createResult.IdOutboxMessage,
			checkPermissions: true,
			cancellationToken: default);

		archiveQuery = new Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery(queue.IdOutboxQueue, true, true, true, null);
		archiveResult = await messageBus.SendAsync(scopeContext, archiveQuery);

		Assert.That(!archiveResult.HasError && archiveResult.Data != null && archiveResult.Data?.Count == 1 && archiveResult.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdOutboxQueue == queue.IdOutboxQueue).IdMessageType);

		messageQuery = new Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueueQuery(queue.IdOutboxQueue, true, true, true, null);
		messageResult = await messageBus.SendAsync(scopeContext, messageQuery);

		Assert.That(!messageResult.HasError && messageResult.Data != null && messageResult.Data?.Count == 1);

		var outboxQueueOutboxMessageArchives = await outboxStore.GetAllOutboxMessageArchivesAsync(
			scopeContext,
			queue.IdOutboxQueue,
			new SortDescriptorBuilder<Model.VwOutboxMessageArchive>()
				.SortBy(x => x.CreatedUtc, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!outboxQueueOutboxMessageArchives.HasError
			&& outboxQueueOutboxMessageArchives.Data != null
			&& outboxQueueOutboxMessageArchives.Data?.TotalCount == 1
			&& outboxQueueOutboxMessageArchives.Data.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdOutboxQueue == queue.IdOutboxQueue).IdMessageType);

		var outboxMessageArchive = await outboxStore.GetOutboxMessageArchiveAsync(scopeContext, createResult.IdOutboxMessage, true, cancellationToken: default);

		Assert.That(
			!outboxMessageArchive.HasError
			&& outboxMessageArchive.Data != null
			&& outboxMessageArchive.Data.IdMessageType == queueResult.Data!.First(x => x.IdOutboxQueue == queue.IdOutboxQueue).IdMessageType);

		var outboxMessageContent = await outboxStore.GetOutboxMessageContentAsync(scopeContext, createResult.IdOutboxMessage, true, cancellationToken: default);

		Assert.That(
			!outboxMessageContent.HasError
			&& outboxMessageContent.Data != null
			&& outboxMessageContent.Data.StringContent == messageText);
	}
}
