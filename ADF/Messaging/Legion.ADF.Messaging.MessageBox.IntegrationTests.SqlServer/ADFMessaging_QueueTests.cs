using Legion.ADF.Messaging.MessageBox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[Category("ADFMessaging Queue tests")]
public class ADFMessaging_QueueTests : TestBase
{
	[Test]
	public async Task Queue_ShouldCreateQueue()
	{
		var idUser = GlobalContext.Instance.NewGuid();
		var tenantIdentifier = GlobalContext.Instance.NewGuid();
		var correlationId = GlobalContext.Instance.NewGuid();
		var externalCorrelationId = GlobalContext.Instance.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var messageBoxStore = sp.GetRequiredService<IMessageBoxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var name = "TestQueue";

		//var reconstructedType = Type.GetType(@namespace);

		var query = new Queries.Queue.GetAllQueuesQuery(false, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 3 && result.Data.Count(x => x.Name == name) == 1);

		var queueMessages = await messageBoxStore.GetQueueMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwQueueMessages>()
				.SortBy(x => x.QueueName, System.ComponentModel.ListSortDirection.Ascending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!queueMessages.HasError
			&& queueMessages.Data != null
			&& queueMessages.Data.Data.FirstOrDefault(x => x.QueueName == name) != null);
	}
}
