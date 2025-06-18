#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public partial class DomainEventsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsDbContext
{
	public override bool IsDomainEventContext => true;
}
