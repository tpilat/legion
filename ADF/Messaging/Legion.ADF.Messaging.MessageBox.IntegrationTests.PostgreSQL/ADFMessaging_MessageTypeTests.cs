using Legion.ADF.Messaging.MessageBox.IntegrationTests.Messages;
using Legion.ADF.Messaging.MessageBox.Services;
using Legion.Extensions;
using Legion.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Legion.ADF.Messaging.MessageBox.IntegrationTests;

[Category("ADFMessaging MessageType tests")]
public class ADFMessaging_MessageTypeTests : TestBase
{
	[Test]
	public async Task MessageType_ShouldCreateMessageType()
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

		var type = typeof(TestMessage);
		var @namespace = type.GetSimplifiedAssemblyQualifiedName();

		var query = new Queries.MessageType.GetMessageTypeByNamespaceQuery(@namespace, true, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null && result.Data?.Namespace == @namespace);
	}
}
