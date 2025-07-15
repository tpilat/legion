using Legion.ADF.Messaging.Inbox.Services;
using Legion.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests;

[Category("ADFMessaging InboxMessageProcessingLog tests")]
public class ADFMessaging_InboxMessageProcessingLogTests : TestBase
{
	[Test]
	public async Task InboxMessageProcessingLog_ShouldCreateInboxMessageProcessingLog()
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

		await using var uow = CreateInboxUnitOfWork(scopeContext, sp);
		var queue = await uow.InboxQueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var query = new Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery(createResult.IdInboxMessage, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 1 && result.Data[0].IdInboxQueue == queue.IdInboxQueue);

		using var inboxStore2 = sp.GetRequiredService<IInboxStore>();
		var logsResult = await inboxStore2.GetInboxMessageProcessingLogsAsync(scopeContext, createResult.IdInboxMessage, null, true, default);

		Assert.That(!logsResult.HasError && logsResult.Data != null && logsResult.Data?.Count == 1 && logsResult.Data[0].IdInboxQueue == queue.IdInboxQueue);
	}
}
