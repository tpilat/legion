using Legion.ADF.Messaging.Settings;
using Legion.Database;
using Legion.Model.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Legion.ADF.Messaging.DomainEvents.Services.Internal;

internal class DomainEventStoreFactory : IDomainEventStoreFactory
{
	public IDomainEventStore Create(IConnectionProvider connectionProvider)
	{
		Throw.IfArgumentNull(connectionProvider);

		var messagingDomainEventsStoreOptions = connectionProvider.ServiceProvider.GetRequiredService<IOptions<MessagingDomainEventsStoreOptions>>();
		var logger = connectionProvider.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DomainEventStore>();

		return new DomainEventStore(connectionProvider, messagingDomainEventsStoreOptions, logger);
	}
}
