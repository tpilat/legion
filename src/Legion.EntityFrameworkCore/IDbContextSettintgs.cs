using Legion.Model.Audit;
using Legion.Model.Messaging;

namespace Legion.EntityFrameworkCore;

public interface IDbContextSettintgs
{
	bool? AllowLocking { get; }
	IAuditEntryStore? AuditEntryStore { get; }
	IDomainEventStore? DomainEventStore { get; }
}
