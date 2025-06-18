using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public interface IDomainEventsDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> BlockedDomainEventType { get; }
	DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> DomainEvent { get; }
	DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent> DomainEventContent { get; }
	DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> DomainEventProcessingLog { get; }
	DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus> DomainEventProcessingStatus { get; }
}
