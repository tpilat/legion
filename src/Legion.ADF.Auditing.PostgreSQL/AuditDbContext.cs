using Legion.EntityFrameworkCore.Audit;
using Legion.Model.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Auditing.PostgreSQL;

public partial class AuditDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Auditing.PostgreSQL.IAuditDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static AuditDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Auditing.Audit.ApplicationEntry), PostgreSQL.Audit.ApplicationEntryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auditing.Audit.ApplicationEntryToken), PostgreSQL.Audit.ApplicationEntryTokenConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auditing.Audit.AuditEntry), PostgreSQL.Audit.AuditEntryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Auditing.Audit.AuditType), PostgreSQL.Audit.AuditTypeConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Auditing.Audit.ApplicationEntry> ApplicationEntry { get; set; }
	public virtual DbSet<Legion.ADF.Auditing.Audit.ApplicationEntryToken> ApplicationEntryToken { get; set; }
	public virtual DbSet<Legion.ADF.Auditing.Audit.AuditEntry> AuditEntry { get; set; }
	public virtual DbSet<Legion.ADF.Auditing.Audit.AuditType> AuditType { get; set; }

	public AuditDbContext(DbContextOptions<AuditDbContext> options, Microsoft.Extensions.Logging.ILogger<AuditDbContext> logger)
		: base(options, logger)
	{
	}

	public AuditDbContext(Microsoft.Extensions.Logging.ILogger<AuditDbContext> logger)
		: base(logger)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
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

		PostgreSQL.Audit.ApplicationEntryConfiguration.Build(modelBuilder);
		PostgreSQL.Audit.ApplicationEntryTokenConfiguration.Build(modelBuilder);
		PostgreSQL.Audit.AuditEntryConfiguration.Build(modelBuilder);
		PostgreSQL.Audit.AuditTypeConfiguration.Build(modelBuilder);
	}
}
