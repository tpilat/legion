using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Audit.SqlServer;

public interface IAuditQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Audit.Model.VwApplicationEntry> VwApplicationEntry { get; set; }
	DbSet<Legion.ADF.Audit.Model.VwAuditEntry> VwAuditEntry { get; set; }
}
