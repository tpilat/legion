using Legion.ADF.Messaging.MessageBox.Services;
using Legion.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[Category("ADFMessaging MessageProcessingLog tests")]
public class ADFMessaging_MessageProcessingLogTests : TestBase
{
	[Test]
	public async Task MessageProcessingLog_ShouldCreateMessageProcessingLog()
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
		var messageDtoCreateResult = await ADFMessaging_MessageTests.CreateMessageAsync(
			scopeContext,
			"myString",
			messageText,
			queueName,
			messageBoxStore);

		await using var uow = CreateMessageBoxUnitOfWork(scopeContext, sp);
		var queuedMessage = await uow.QueuedMessageRepository
			.AsQueryable(scopeContext)
			.AsNoTracking()
			.Where(x => x.IdMessage == messageDtoCreateResult.IdMessage)
			.FirstOrDefaultAsync(cancellationToken: default);

		Assert.IsNotNull(queuedMessage, nameof(queuedMessage));

		var query = new Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery(messageDtoCreateResult.IdMessage, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 1 && result.Data[0].IdQueuedMessage == queuedMessage.IdQueuedMessage);

		using var messageBoxStore2 = sp.GetRequiredService<IMessageBoxStore>();
		var logsResult = await messageBoxStore2.GetMessageProcessingLogsAsync(scopeContext, messageDtoCreateResult.IdMessage, null, true, default);

		Assert.That(!logsResult.HasError && logsResult.Data != null && logsResult.Data?.Count == 1 && logsResult.Data[0].IdQueuedMessage == queuedMessage.IdQueuedMessage);
	}
}
