using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Audit.SqlServer;

public partial class AuditDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Audit.SqlServer.IAuditDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static AuditDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntry), SqlServer.ApplicationEntryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntryRequest), SqlServer.ApplicationEntryRequestConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntryResponse), SqlServer.ApplicationEntryResponseConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.ApplicationEntryToken), SqlServer.ApplicationEntryTokenConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.AuditEntry), SqlServer.AuditEntryConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Audit.Model.AuditOperation), SqlServer.AuditOperationConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.ApplicationEntryConfiguration.Build(modelBuilder);
		SqlServer.ApplicationEntryRequestConfiguration.Build(modelBuilder);
		SqlServer.ApplicationEntryResponseConfiguration.Build(modelBuilder);
		SqlServer.ApplicationEntryTokenConfiguration.Build(modelBuilder);
		SqlServer.AuditEntryConfiguration.Build(modelBuilder);
		SqlServer.AuditOperationConfiguration.Build(modelBuilder);
	}
}
