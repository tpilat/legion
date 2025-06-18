using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Auditing.PostgreSQL;

public interface IAuditDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Auditing.Audit.ApplicationEntry> ApplicationEntry { get; }
	DbSet<Legion.ADF.Auditing.Audit.ApplicationEntryToken> ApplicationEntryToken { get; }
	DbSet<Legion.ADF.Auditing.Audit.AuditEntry> AuditEntry { get; }
	DbSet<Legion.ADF.Auditing.Audit.AuditType> AuditType { get; }
}
