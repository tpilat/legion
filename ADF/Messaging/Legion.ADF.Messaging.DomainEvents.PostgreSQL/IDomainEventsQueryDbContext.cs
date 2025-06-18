using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public interface IDomainEventsQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent> VwDomainEvent { get; set; }
}
