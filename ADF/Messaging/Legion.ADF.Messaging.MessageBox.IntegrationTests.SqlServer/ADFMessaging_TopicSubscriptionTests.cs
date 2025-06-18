using Legion.ADF.Messaging.MessageBox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[Category("ADFMessaging TopicSubscription tests")]
public class ADFMessaging_TopicSubscriptionTests : TestBase
{
	[Test]
	public async Task TopicSubscription_ShouldCreateTopicSubscription()
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

		var subscriptionName = "TestTopicSubscription";

		//var reconstructedType = Type.GetType(@namespace);

		var query = new Queries.TopicSubscription.GetAllTopicSubscriptionsQuery(false, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 3 && result.Data.Count(x => x.SubscriptionName == subscriptionName) == 1);

		var subscribedMessages = await messageBoxStore.GetTopicSubscriptionMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwTopicSubscriptionMessages>()
				.SortBy(x => x.SubscriptionName, System.ComponentModel.ListSortDirection.Ascending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!subscribedMessages.HasError
			&& subscribedMessages.Data != null
			&& subscribedMessages.Data.Data.FirstOrDefault(x => x.SubscriptionName == subscriptionName) != null);
	}
}
