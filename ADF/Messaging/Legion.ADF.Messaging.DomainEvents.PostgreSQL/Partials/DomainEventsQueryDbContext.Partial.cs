#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public partial class DomainEventsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsQueryDbContext
{
	public override bool IsDomainEventContext => true;
}
