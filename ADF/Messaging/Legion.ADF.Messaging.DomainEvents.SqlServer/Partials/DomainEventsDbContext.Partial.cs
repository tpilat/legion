#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public partial class DomainEventsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext
{
	public override bool IsDomainEventContext => true;
}
