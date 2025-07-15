using Legion.ADF.Messaging.Inbox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests;

[NUnit.Framework.Category("ADFMessaging InboxMessage tests")]
public class ADFMessaging_InboxMessageTests : TestBase
{
	internal static async Task<(DTOs.InboxMessageDto InboxMessageDto, Guid IdInboxMessage)> CreateInboxMessageAsync(
		IScopeContext scopeContext,
		string @namespace,
		string message,
		string queueName,
		IInboxStore inboxStore)
	{
		scopeContext = ScopeContext.Create(scopeContext);
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		var inboxMessageDto = DTOs.InboxMessageDto.CreateStringMessage(
			@namespace,
			message,
			 GlobalContext.Instance.NewGuid().ToString(),
			"TargetTopic",
			 GlobalContext.Instance.NewGuid().ToString(),
			 targetTopic: null,
			 "TargetQueueName");

		var createResult = await inboxStore.CreateInboxMessageAsync(
			scopeContext,
			inboxMessageDto,
			queueName,
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		return (inboxMessageDto, createResult.Data);
	}

	[Test]
	public async Task InboxMessage_ShouldCreateInboxMessage()
	{
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var tmpResult = await ADFMessaging_InboxMessageTests.CreateInboxMessageAsync(
			scopeContext,
			"myString",
			"fakeTxt",
			"FakeQueue",
			inboxStore);

		var queueName = "TestQueue";

		var messageText = "TestText";
		var createResult = await ADFMessaging_InboxMessageTests.CreateInboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			inboxStore);

		createResult = await ADFMessaging_InboxMessageTests.CreateInboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			inboxStore);

		var queueQuery = new Queries.InboxQueue.GetAllInboxQueuesQuery(false, true, true, true, null);
		var queueResult = await messageBus.SendAsync(scopeContext, queueQuery);

		Assert.That(!queueResult.HasError && queueResult.Data != null && queueResult.Data?.Count == 3);

		await using var uow = CreateInboxUnitOfWork(scopeContext, sp);
		var queue = await uow.InboxQueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var query = new Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueueQuery(queue.IdInboxQueue, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.Count(x => x.IdMessageType == queueResult.Data!.First(x => x.IdInboxQueue == queue.IdInboxQueue).IdMessageType) == 2);

		var inboxQueueMessages = await inboxStore.GetInboxQueueMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwInboxQueueMessages>()
				.SortBy(x => x.InboxQueueName, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!inboxQueueMessages.HasError
			&& inboxQueueMessages.Data != null
			&& inboxQueueMessages.Data?.TotalCount == 3
			&& inboxQueueMessages.Data.Data.First(q => q.IdInboxQueue == queue.IdInboxQueue).CreatedMessageCount == 2);

		var inboxQueueInboxMessages = await inboxStore.GetAllInboxMessagesAsync(
			scopeContext,
			queue.IdInboxQueue,
			new SortDescriptorBuilder<Model.VwInboxMessage>()
				.SortBy(x => x.CreatedUtc, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!inboxQueueInboxMessages.HasError
			&& inboxQueueInboxMessages.Data != null
			&& inboxQueueInboxMessages.Data?.TotalCount == 2
			&& inboxQueueInboxMessages.Data.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdInboxQueue == queue.IdInboxQueue).IdMessageType);

		var inboxMessage = await inboxStore.GetInboxMessageAsync(scopeContext, createResult.IdInboxMessage, true, cancellationToken: default);

		Assert.That(
			!inboxMessage.HasError
			&& inboxMessage.Data != null
			&& inboxMessage.Data.IdMessageType == queueResult.Data!.First(x => x.IdInboxQueue == queue.IdInboxQueue).IdMessageType);

		var inboxMessageContent = await inboxStore.GetInboxMessageContentAsync(scopeContext, createResult.IdInboxMessage, true, cancellationToken: default);

		Assert.That(
			!inboxMessageContent.HasError
			&& inboxMessageContent.Data != null
			&& inboxMessageContent.Data.StringContent == messageText);
	}

	[Test]
	public async Task InboxMessage_ShouldArchivateInboxMessage()
	{
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var tmpResult = await ADFMessaging_InboxMessageTests.CreateInboxMessageAsync(
			scopeContext,
			"myString",
			"fakeTxt",
			"FakeQueue",
			inboxStore);

		var queueName = "TestQueue";

		var messageText = "TestText";
		var createResult = await ADFMessaging_InboxMessageTests.CreateInboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			inboxStore);

		createResult = await ADFMessaging_InboxMessageTests.CreateInboxMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			inboxStore);

		var queueQuery = new Queries.InboxQueue.GetAllInboxQueuesQuery(false, true, true, true, null);
		var queueResult = await messageBus.SendAsync(scopeContext, queueQuery);

		Assert.That(!queueResult.HasError && queueResult.Data != null && queueResult.Data?.Count == 3);

		await using var uow = CreateInboxUnitOfWork(scopeContext, sp);
		var queue = await uow.InboxQueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var messageQuery = new Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueueQuery(queue.IdInboxQueue, true, true, true, null);
		var messageResult = await messageBus.SendAsync(scopeContext, messageQuery);

		Assert.That(!messageResult.HasError && messageResult.Data != null && messageResult.Data.Count(x => x.IdMessageType == queueResult.Data!.First(x => x.IdInboxQueue == queue.IdInboxQueue).IdMessageType) == 2);

		var archiveQuery = new Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery(queue.IdInboxQueue, true, true, true, null);
		var archiveResult = await messageBus.SendAsync(scopeContext, archiveQuery);

		Assert.That(!archiveResult.HasError && archiveResult.Data != null && archiveResult.Data.Count == 0);

		var createArchiveResult = await inboxStore.ArchivateInboxMessageAsync(
			scopeContext,
			createResult.IdInboxMessage,
			checkPermissions: true,
			cancellationToken: default);

		archiveQuery = new Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery(queue.IdInboxQueue, true, true, true, null);
		archiveResult = await messageBus.SendAsync(scopeContext, archiveQuery);

		Assert.That(!archiveResult.HasError && archiveResult.Data != null && archiveResult.Data?.Count == 1 && archiveResult.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdInboxQueue == queue.IdInboxQueue).IdMessageType);

		messageQuery = new Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueueQuery(queue.IdInboxQueue, true, true, true, null);
		messageResult = await messageBus.SendAsync(scopeContext, messageQuery);

		Assert.That(!messageResult.HasError && messageResult.Data != null && messageResult.Data?.Count == 1);

		var inboxQueueInboxMessageArchives = await inboxStore.GetAllInboxMessageArchivesAsync(
			scopeContext,
			queue.IdInboxQueue,
			new SortDescriptorBuilder<Model.VwInboxMessageArchive>()
				.SortBy(x => x.CreatedUtc, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!inboxQueueInboxMessageArchives.HasError
			&& inboxQueueInboxMessageArchives.Data != null
			&& inboxQueueInboxMessageArchives.Data?.TotalCount == 1
			&& inboxQueueInboxMessageArchives.Data.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdInboxQueue == queue.IdInboxQueue).IdMessageType);

		var inboxMessageArchive = await inboxStore.GetInboxMessageArchiveAsync(scopeContext, createResult.IdInboxMessage, true, cancellationToken: default);

		Assert.That(
			!inboxMessageArchive.HasError
			&& inboxMessageArchive.Data != null
			&& inboxMessageArchive.Data.IdMessageType == queueResult.Data!.First(x => x.IdInboxQueue == queue.IdInboxQueue).IdMessageType);

		var inboxMessageContent = await inboxStore.GetInboxMessageContentAsync(scopeContext, createResult.IdInboxMessage, true, cancellationToken: default);

		Assert.That(
			!inboxMessageContent.HasError
			&& inboxMessageContent.Data != null
			&& inboxMessageContent.Data.StringContent == messageText);
	}
}
