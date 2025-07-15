using Legion.ADF.Messaging.Outbox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests;

[Category("ADFMessaging OutboxQueue tests")]
public class ADFMessaging_OutboxQueueTests : TestBase
{
	[Test]
	public async Task OutboxQueue_ShouldCreateOutboxQueue()
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

		var name = "TestQueue";

		//var reconstructedType = Type.GetType(@namespace);

		var query = new Queries.OutboxQueue.GetAllOutboxQueuesQuery(false, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 3 && result.Data.Count(x => x.Name == name) == 1);

		var outboxQueueMessages = await outboxStore.GetOutboxQueueMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwOutboxQueueMessages>()
				.SortBy(x => x.OutboxQueueName, System.ComponentModel.ListSortDirection.Ascending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!outboxQueueMessages.HasError
			&& outboxQueueMessages.Data != null
			&& outboxQueueMessages.Data.Data.FirstOrDefault(x => x.OutboxQueueName == name) != null);
	}
}
