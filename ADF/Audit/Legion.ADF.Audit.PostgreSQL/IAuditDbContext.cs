using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Audit.PostgreSQL;

public interface IAuditDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Audit.Model.ApplicationEntry> ApplicationEntry { get; }
	DbSet<Legion.ADF.Audit.Model.ApplicationEntryRequest> ApplicationEntryRequest { get; }
	DbSet<Legion.ADF.Audit.Model.ApplicationEntryResponse> ApplicationEntryResponse { get; }
	DbSet<Legion.ADF.Audit.Model.ApplicationEntryToken> ApplicationEntryToken { get; }
	DbSet<Legion.ADF.Audit.Model.AuditEntry> AuditEntry { get; }
	DbSet<Legion.ADF.Audit.Model.AuditOperation> AuditOperation { get; }
}
