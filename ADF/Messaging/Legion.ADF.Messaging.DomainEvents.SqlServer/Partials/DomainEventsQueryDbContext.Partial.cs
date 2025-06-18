#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public partial class DomainEventsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsQueryDbContext
{
	public override bool IsDomainEventContext => true;
}
