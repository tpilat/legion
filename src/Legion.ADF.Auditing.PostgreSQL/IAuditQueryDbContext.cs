using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Auditing.PostgreSQL;

public interface IAuditQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Auditing.Audit.VwApplicationEntry> VwApplicationEntry { get; set; }
	DbSet<Legion.ADF.Auditing.Audit.VwApplicationEntryToken> VwApplicationEntryToken { get; set; }
	DbSet<Legion.ADF.Auditing.Audit.VwAuditEntry> VwAuditEntry { get; set; }
}
