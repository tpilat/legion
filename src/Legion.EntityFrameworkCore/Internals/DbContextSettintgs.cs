using Legion.Model.Audit;
using Legion.Model.Messaging;

namespace Legion.EntityFrameworkCore.Internals;

internal class DbContextSettintgs : IDbContextSettintgs
{
	public bool? AllowLocking { get; set; }
	public IAuditEntryStore? AuditEntryStore { get; set; }
	public IDomainEventStore? DomainEventStore { get; set; }
}
