using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Auditing.PostgreSQL;

public partial class AuditQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Auditing.PostgreSQL.IAuditQueryDbContext
{
	public virtual DbSet<Legion.ADF.Auditing.Audit.VwApplicationEntry> VwApplicationEntry { get; set; }
	public virtual DbSet<Legion.ADF.Auditing.Audit.VwApplicationEntryToken> VwApplicationEntryToken { get; set; }
	public virtual DbSet<Legion.ADF.Auditing.Audit.VwAuditEntry> VwAuditEntry { get; set; }

	public AuditQueryDbContext(DbContextOptions<AuditQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<AuditQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public AuditQueryDbContext(Microsoft.Extensions.Logging.ILogger<AuditQueryDbContext> logger)
		: base(logger)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		
		if (!optionsBuilder.IsConfigured)
		{
			if (ConnectionProvider == null)
				Legion.Throw.InitializationException(ConnectionProvider);

			ConnectionProvider.OnConfiguring(optionsBuilder);
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.Audit.VwApplicationEntryConfiguration.Build(modelBuilder);
		PostgreSQL.Audit.VwApplicationEntryTokenConfiguration.Build(modelBuilder);
		PostgreSQL.Audit.VwAuditEntryConfiguration.Build(modelBuilder);
	}
}
