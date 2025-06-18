using Legion.Database;

namespace Legion.Model.Messaging;

public interface IDomainEventStoreFactory
{
	IDomainEventStore Create(IConnectionProvider connectionProvider);
}
