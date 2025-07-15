using Legion.ADF.Messaging.Outbox.Services;
using Legion.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests;

[Category("ADFMessaging OutboxMessageProcessingLog tests")]
public class ADFMessaging_OutboxMessageProcessingLogTests : TestBase
{
	[Test]
	public async Task OutboxMessageProcessingLog_ShouldCreateOutboxMessageProcessingLog()
	{
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

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

		await using var uow = CreateOutboxUnitOfWork(scopeContext, sp);
		var queue = await uow.OutboxQueueRepository
			.AsQueryable(scopeContext)
			.Where(x => x.Name == queueName)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queue, nameof(queue));

		var query = new Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery(createResult.IdOutboxMessage, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 1 && result.Data[0].IdOutboxQueue == queue.IdOutboxQueue);

		using var outboxStore2 = sp.GetRequiredService<IOutboxStore>();
		var logsResult = await outboxStore2.GetOutboxMessageProcessingLogsAsync(scopeContext, createResult.IdOutboxMessage, null, true, default);

		Assert.That(!logsResult.HasError && logsResult.Data != null && logsResult.Data?.Count == 1 && logsResult.Data[0].IdOutboxQueue == queue.IdOutboxQueue);
	}
}
