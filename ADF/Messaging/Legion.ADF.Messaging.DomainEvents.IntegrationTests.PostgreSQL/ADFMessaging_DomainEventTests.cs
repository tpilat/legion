using Legion.ADF.Messaging.DomainEvents.IntegrationTests.PostgreSQL;
using Legion.ADF.Messaging.DomainEvents.Services;
using Legion.Database;
using Legion.Extensions;
using Legion.MessageBus;
using Legion.Reflection;
using Legion.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Legion.ADF.Messaging.DomainEvents.IntegrationTests;

[Category("ADFMessaging DomainEvents tests")]
public class ADFMessaging_DomainEventTests : TestBase
{
	[Test]
	public async Task DomainEvent_ShouldCreateDomainEvent()
	{
		//reset CACHED blocked domain event namespaces
		new ObjectWrapper<DomainEventStore>(null)["_blockedDomainEventNamespaces"] = null;

		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();

		using var domainEventsStore = sp.GetRequiredService<DomainEventStore>();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var domainEvent = new TestDomainEvent("MyMessageContent");

		var createResult = await domainEventsStore.SaveDomainEventAsync(
			scopeContext,
			domainEvent,
			propertiesJson: null,
			"TestCase",
			"001",
			checkMessageExists: true,
			checkPermissions: true,
			cancellationToken: default);

		Assert.That(!createResult.HasError && createResult.Data != null);

		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var query = new Queries.DomainEventContent.GetDomainEventContentByIdQuery(domainEvent.Id, true, true, null);
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data != null);

		var domainEventNamespace = domainEvent.GetType().GetSimplifiedAssemblyQualifiedName();
		var domainEventType = Type.GetType(domainEventNamespace);
		var dbDomainEvent = JsonSerializerHelper.Deserialize(
			result.Data!.Content,
			domainEventType!,
			new Newtonsoft.Json.JsonSerializerSettings
			{
				Formatting = Formatting.None,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
				PreserveReferencesHandling = PreserveReferencesHandling.Objects, //PreserveReferencesHandling.All,
				TypeNameHandling = TypeNameHandling.All,
				MaxDepth = 255,
				ContractResolver = new Legion.Serializer.JsonConverters.PrivateSetterContractResolver()
			});

		Assert.That(domainEvent.IsTheSame(dbDomainEvent!));

		//GC.Collect();
		//GC.WaitForPendingFinalizers();

		//await Task.Delay(10000);

		//GC.Collect();
		//GC.WaitForPendingFinalizers();

		//var objectsLifetimes = Trackers.ObjectLifetimeTracker.GetObjectsLifetimeStatus();
	}

	[Test]
	public async Task DomainEvent_ShouldNotCreateBlockedDomainEvent()
	{
		//reset CACHED blocked domain event namespaces
		new ObjectWrapper<DomainEventStore>(null)["_blockedDomainEventNamespaces"] = null;

		var idUser = Guid.NewGuid();
		var tenantIdentifier = Guid.NewGuid();
		var correlationId = Guid.NewGuid();
		var externalCorrelationId = Guid.NewGuid().ToString();

		var sp = await SetUp.CreateScopedServiceProviderAsync();
		var scopeContext = ScopeContext.Create("TEST ScopeContext", correlationId: correlationId, externalCorrelationId: externalCorrelationId)
			.AppendTraceFrameWithIduser(idUser, true)
			.AppendTraceFrameWithTenantIdentifier(tenantIdentifier, true);

		var domainEvent = new TestDomainEvent("MyMessageContent");

		//INSERT BlockedDomainEventType
		var connectionProviderFactory = sp.GetRequiredService<IConnectionProviderFactory>();
		await using (var connectionProvider = connectionProviderFactory!.CreateWithNewTransactionByStoreId<ConnectionStringProvider>(
			sp,
			storeId: null,
			transactionIsolationLevel: null,
			allowLocking: false,
			createAuditEntryStore: false))
		{
			var domainEventsUowResult = connectionProvider.UnitOfWorkProvider.Create<IDomainEventsUnitOfWork>(scopeContext);

			if (domainEventsUowResult.HasError)
				domainEventsUowResult.ThrowIfErrorOrNullData(scopeContext, Legion.ADF.Messaging.Exceptions.Internal.ErrorCodes.DomainEventsUnitOfWorkException.InvalidUoW, true);

			var uow = domainEventsUowResult.Data!;
			var blockedDEResult = Model.BlockedDomainEventType.Create(scopeContext, domainEvent.Namespace);

			Assert.That(!blockedDEResult.HasError && blockedDEResult.Data != null);

			uow.BlockedDomainEventTypeRepository.Add(
				scopeContext,
				blockedDEResult.Data!);

			var blockDEResult = await uow.SaveAsync(scopeContext, cancellationToken: default);

			Assert.That(!blockDEResult.HasError && blockDEResult.Data == 1);

			await connectionProvider.CommitAllAsync(scopeContext, cancellationToken: default);
		}

		await using (var domainEventsStore = sp.GetRequiredService<DomainEventStore>())
		{
			var createResult = await domainEventsStore.SaveDomainEventAsync(
				scopeContext,
				domainEvent,
				propertiesJson: null,
				"TestCase",
				"001",
				checkMessageExists: true,
				checkPermissions: true,
				cancellationToken: default);

			Assert.That(!createResult.HasError && !createResult.Data.HasValue, $"createResult.Data = {createResult.Data}");
		}

		var query = new Queries.DomainEvent.ExistsDomainEventByIdDomainEventQuery(domainEvent.Id, true, true, null);
		var messageBus = sp.GetRequiredService<IMessageBus<ConnectionStringProvider>>();
		var result = await messageBus.SendAsync(scopeContext, query);

		Assert.That(!result.HasError && result.Data == false, $"result.Data = {result.Data}");
	}
}
