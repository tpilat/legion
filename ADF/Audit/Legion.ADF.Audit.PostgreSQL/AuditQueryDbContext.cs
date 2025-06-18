using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Audit.PostgreSQL;

public partial class AuditQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Audit.PostgreSQL.IAuditQueryDbContext
{
	public virtual DbSet<Legion.ADF.Audit.Model.VwApplicationEntry> VwApplicationEntry { get; set; }
	public virtual DbSet<Legion.ADF.Audit.Model.VwAuditEntry> VwAuditEntry { get; set; }

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
		else
		{
			SetIsDbContextOptionsBuilderPreconfigured();
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.VwApplicationEntryConfiguration.Build(modelBuilder);
		PostgreSQL.VwAuditEntryConfiguration.Build(modelBuilder);
	}
}
