using Legion.ADF.Messaging.Outbox.IntegrationTests.Messages;
using Legion.ADF.Messaging.Outbox.Services;
using Legion.Extensions;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.Outbox.IntegrationTests;

[Category("ADFMessaging OutboxMessageType tests")]
public class ADFMessaging_OutboxMessageTypeTests : TestBase
{
	[Test]
	public async Task OutboxMessageType_ShouldCreateOutboxMessageType()
	{
		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();

		using var outboxStore = sp.GetRequiredService<IOutboxStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var type = typeof(TestMessage);
		var @namespace = type.GetSimplifiedAssemblyQualifiedName();

		var query = new Queries.OutboxMessageType.GetOutboxMessageTypeByNamespaceQuery(@namespace, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Namespace == @namespace);
	}
}
