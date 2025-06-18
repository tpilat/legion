using Legion.ADF.Messaging.MessageBox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[NUnit.Framework.Category("ADFMessaging Message tests")]
public class ADFMessaging_MessageTests : TestBase
{
	internal static async Task<(DTOs.MessageBoxMessageDto MessageDto, Guid IdMessage)> CreateMessageAsync(
		IScopeContext scopeContext,
		string @namespace,
		string message,
		string queueName,
		IMessageBoxStore messageBoxStore)
	{
		scopeContext = ScopeContext.Create(scopeContext);
		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		var messageDto = DTOs.MessageBoxMessageDto.CreateStringMessage(
			@namespace,
			message,
			 Guid.NewGuid().ToString(),
			"TargetTopic",
			 Guid.NewGuid().ToString());

		var createResult = await messageBoxStore.CreateMessageAsync(
			scopeContext,
			messageDto,
			queueName,
			topicName: null,
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		createResult.ThrowIfErrorOrNullData(scopeContext, null, true);

		return (messageDto, createResult.Data);
	}

	[Test]
	public async Task Message_ShouldCreateMessage()
	{
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var tmpResult = await ADFMessaging_MessageTests.CreateMessageAsync(
			scopeContext,
			"myString",
			"fakeTxt",
			"FakeQueue",
			messageBoxStore);

		var queueName = "TestQueue";

		var messageText = "TestText";
		var createResult = await ADFMessaging_MessageTests.CreateMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			messageBoxStore);

		createResult = await ADFMessaging_MessageTests.CreateMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			messageBoxStore);

		var queueQuery = new Queries.Queue.GetAllQueuesQuery(false, true, true, true, null);
		var queueResult = await messageBus.SendAsync(scopeContext, queueQuery);

		Assert.That(!queueResult.HasError && queueResult.Data != null && queueResult.Data?.Count == 3);

		await using var uow = CreateMessageBoxUnitOfWork(scopeContext, sp);
		var queue = await uow.QueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var query = new Queries.VwMessage.GetAllVwMessagesByIdQueueQuery(queue.IdQueue, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data.Count(x => x.IdMessageType == queueResult.Data!.First(x => x.IdQueue == queue.IdQueue).IdMessageType) == 2);

		var queueMessages = await messageBoxStore.GetQueueMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwQueueMessages>()
				.SortBy(x => x.QueueName, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!queueMessages.HasError
			&& queueMessages.Data != null
			&& queueMessages.Data?.TotalCount == 3
			&& queueMessages.Data.Data.First(q => q.IdQueue == queue.IdQueue).AssignedMessageCount == 2);

		var messages = await messageBoxStore.GetAllQueuedMessagesAsync(
			scopeContext,
			queue.IdQueue,
			new SortDescriptorBuilder<Model.VwMessage>()
				.SortBy(x => x.CreatedUtc, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!messages.HasError
			&& messages.Data != null
			&& messages.Data?.TotalCount == 2
			&& messages.Data.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdQueue == queue.IdQueue).IdMessageType);

		var message = await messageBoxStore.GetMessageAsync(scopeContext, createResult.IdMessage, true, cancellationToken: default);

		Assert.That(
			!message.HasError
			&& message.Data != null
			&& message.Data.IdMessageType == queueResult.Data!.First(x => x.IdQueue == queue.IdQueue).IdMessageType);

		var messageContent = await messageBoxStore.GetMessageContentAsync(scopeContext, createResult.IdMessage, true, cancellationToken: default);

		Assert.That(
			!messageContent.HasError
			&& messageContent.Data != null
			&& messageContent.Data.StringContent == messageText);
	}

	[Test]
	public async Task Message_ShouldArchivateMessage()
	{
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var tmpResult = await ADFMessaging_MessageTests.CreateMessageAsync(
			scopeContext,
			"myString",
			"fakeTxt",
			"FakeQueue",
			messageBoxStore);

		var queueName = "TestQueue";

		var messageText = "TestText";
		var createResult = await ADFMessaging_MessageTests.CreateMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			messageBoxStore);

		createResult = await ADFMessaging_MessageTests.CreateMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			messageBoxStore);

		var queueQuery = new Queries.Queue.GetAllQueuesQuery(false, true, true, true, null);
		var queueResult = await messageBus.SendAsync(scopeContext, queueQuery);

		Assert.That(!queueResult.HasError && queueResult.Data != null && queueResult.Data?.Count == 3);

		await using var uow = CreateMessageBoxUnitOfWork(scopeContext, sp);
		var queue = await uow.QueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var messageQuery = new Queries.VwMessage.GetAllVwMessagesByIdQueueQuery(queue.IdQueue, true, true, true, null);
		var messageResult = await messageBus.SendAsync(scopeContext, messageQuery);

		Assert.That(!messageResult.HasError && messageResult.Data != null && messageResult.Data.Count(x => x.IdMessageType == queueResult.Data!.First(x => x.IdQueue == queue.IdQueue).IdMessageType) == 2);

		var archiveQuery = new Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery(queue.IdQueue, true, true, true, null);
		var archiveResult = await messageBus.SendAsync(scopeContext, archiveQuery);

		Assert.That(!archiveResult.HasError && archiveResult.Data != null && archiveResult.Data.Count == 0);

		var createArchiveResult = await messageBoxStore.ArchivateMessageAsync(
			scopeContext,
			createResult.IdMessage,
			checkPermissions: true,
			cancellationToken: default);

		archiveQuery = new Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery(queue.IdQueue, true, true, true, null);
		archiveResult = await messageBus.SendAsync(scopeContext, archiveQuery);

		Assert.That(!archiveResult.HasError && archiveResult.Data != null && archiveResult.Data?.Count == 1 && archiveResult.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdQueue == queue.IdQueue).IdMessageType);

		messageQuery = new Queries.VwMessage.GetAllVwMessagesByIdQueueQuery(queue.IdQueue, true, true, true, null);
		messageResult = await messageBus.SendAsync(scopeContext, messageQuery);

		Assert.That(!messageResult.HasError && messageResult.Data != null && messageResult.Data?.Count == 1);

		var queueMessageArchives = await messageBoxStore.GetAllQueuedMessageArchivesAsync(
			scopeContext,
			queue.IdQueue,
			new SortDescriptorBuilder<Model.VwMessageArchive>()
				.SortBy(x => x.CreatedUtc, ListSortDirection.Descending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!queueMessageArchives.HasError
			&& queueMessageArchives.Data != null
			&& queueMessageArchives.Data?.TotalCount == 1
			&& queueMessageArchives.Data.Data[0].IdMessageType == queueResult.Data!.First(x => x.IdQueue == queue.IdQueue).IdMessageType);

		var messageArchive = await messageBoxStore.GetMessageArchiveAsync(scopeContext, createResult.IdMessage, true, cancellationToken: default);

		Assert.That(
			!messageArchive.HasError
			&& messageArchive.Data != null
			&& messageArchive.Data.IdMessageType == queueResult.Data!.First(x => x.IdQueue == queue.IdQueue).IdMessageType);

		var messageContent = await messageBoxStore.GetMessageContentAsync(scopeContext, createResult.IdMessage, true, cancellationToken: default);

		Assert.That(
			!messageContent.HasError
			&& messageContent.Data != null
			&& messageContent.Data.StringContent == messageText);
	}
}
