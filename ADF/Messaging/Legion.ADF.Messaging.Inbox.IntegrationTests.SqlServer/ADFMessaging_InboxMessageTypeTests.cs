using Legion.ADF.Messaging.Inbox.IntegrationTests.Messages;
using Legion.ADF.Messaging.Inbox.Services;
using Legion.Extensions;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Inbox.IntegrationTests;

[Category("ADFMessaging InboxMessageType tests")]
public class ADFMessaging_InboxMessageTypeTests : TestBase
{
	[Test]
	public async Task InboxMessageType_ShouldCreateInboxMessageType()
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

		var type = typeof(TestMessage);
		var @namespace = type.GetSimplifiedAssemblyQualifiedName();

		var query = new Queries.InboxMessageType.GetInboxMessageTypeByNamespaceQuery(@namespace, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Namespace == @namespace);
	}
}
