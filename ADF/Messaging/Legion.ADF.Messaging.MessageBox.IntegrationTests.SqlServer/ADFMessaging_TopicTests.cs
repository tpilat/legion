using Legion.ADF.Messaging.MessageBox.Services;
using Legion.MessageBus;
using Legion.Queries.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[Category("ADFMessaging Topic tests")]
public class ADFMessaging_TopicTests : TestBase
{
	[Test]
	public async Task Topic_ShouldCreateTopic()
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

		var name = "TestTopic";

		//var reconstructedType = Type.GetType(@namespace);

		var query = new Queries.Topic.GetAllTopicsQuery(false, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Count == 3 && result.Data.Count(x => x.Name == name) == 1);

		var subscribedMessages = await messageBoxStore.GetTopicSubscriptionMessagesAsync(
			scopeContext,
			false,
			new SortDescriptorBuilder<Model.VwTopicSubscriptionMessages>()
				.SortBy(x => x.TopicName, System.ComponentModel.ListSortDirection.Ascending),
			0,
			100,
			true,
			cancellationToken: default);

		Assert.That(
			!subscribedMessages.HasError
			&& subscribedMessages.Data != null
			&& subscribedMessages.Data.Data.FirstOrDefault(x => x.TopicName == name) != null);
	}
}
