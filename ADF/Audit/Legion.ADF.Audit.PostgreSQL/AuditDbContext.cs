using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Audit.PostgreSQL;

public partial class AuditDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Audit.PostgreSQL.IAuditDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static AuditDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntry), PostgreSQL.ApplicationEntryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest), PostgreSQL.ApplicationEntryRequestConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse), PostgreSQL.ApplicationEntryResponseConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntryToken), PostgreSQL.ApplicationEntryTokenConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.AuditEntry), PostgreSQL.AuditEntryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.AuditOperation), PostgreSQL.AuditOperationConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Audit.Model.ApplicationEntry> ApplicationEntry { get; set; }
	public virtual DbSet<Legion.ADF.Audit.Model.ApplicationEntryRequest> ApplicationEntryRequest { get; set; }
	public virtual DbSet<Legion.ADF.Audit.Model.ApplicationEntryResponse> ApplicationEntryResponse { get; set; }
	public virtual DbSet<Legion.ADF.Audit.Model.ApplicationEntryToken> ApplicationEntryToken { get; set; }
	public virtual DbSet<Legion.ADF.Audit.Model.AuditEntry> AuditEntry { get; set; }
	public virtual DbSet<Legion.ADF.Audit.Model.AuditOperation> AuditOperation { get; set; }

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
		else
		{
			SetIsDbContextOptionsBuilderPreconfigured();
		}

		if (DbContextSettintgs.AllowLocking == true)
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_PostgreSql());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.ApplicationEntryConfiguration.Build(modelBuilder);
		PostgreSQL.ApplicationEntryRequestConfiguration.Build(modelBuilder);
		PostgreSQL.ApplicationEntryResponseConfiguration.Build(modelBuilder);
		PostgreSQL.ApplicationEntryTokenConfiguration.Build(modelBuilder);
		PostgreSQL.AuditEntryConfiguration.Build(modelBuilder);
		PostgreSQL.AuditOperationConfiguration.Build(modelBuilder);
	}
}
