using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Messaging.DomainEvents;

public partial interface IDomainEventsUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Messaging.DomainEvents.Model.Repositories.IBlockedDomainEventTypeRepository BlockedDomainEventTypeRepository { get; }

	Legion.ADF.Messaging.DomainEvents.Model.Repositories.IDomainEventRepository DomainEventRepository { get; }

	Legion.ADF.Messaging.DomainEvents.Model.Repositories.IDomainEventContentRepository DomainEventContentRepository { get; }

	Legion.ADF.Messaging.DomainEvents.Model.Repositories.IDomainEventProcessingLogRepository DomainEventProcessingLogRepository { get; }

	Legion.ADF.Messaging.DomainEvents.Model.Repositories.IDomainEventProcessingStatusRepository DomainEventProcessingStatusRepository { get; }
}
