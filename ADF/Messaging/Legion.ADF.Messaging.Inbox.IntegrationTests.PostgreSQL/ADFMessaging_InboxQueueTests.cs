using Legion.ADF.Messaging.Inbox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests;

[Category("ADFMessaging InboxQueue tests")]
public class ADFMessaging_InboxQueueTests : TestBase
{
	[Test]
	public async Task InboxQueue_ShouldCreateInboxQueue()
	{
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var inboxStore = sp.GetRequiredService<IInboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var name = "TestQueue";

		//var reconstructedType = Type.GetType(@namespace);

		var query = new Queries.InboxQueue.GetAllInboxQueuesQuery(false, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 3 && result.Data.Count(x => x.Name == name) == 1);

		var inboxQueueMessages = await inboxStore.GetInboxQueueMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwInboxQueueMessages>()
				.SortBy(x => x.InboxQueueName, System.ComponentModel.ListSortDirection.Ascending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!inboxQueueMessages.HasError
			&& inboxQueueMessages.Data != null
			&& inboxQueueMessages.Data.Data.FirstOrDefault(x => x.InboxQueueName == name) != null);
	}
}
