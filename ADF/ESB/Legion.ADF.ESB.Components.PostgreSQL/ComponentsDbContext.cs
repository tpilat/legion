using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ESB.Components.PostgreSQL;

public partial class ComponentsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ESB.Components.PostgreSQL.IComponentsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static ComponentsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ESB.Components.Model.Adapter), PostgreSQL.AdapterConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.AdapterLog), PostgreSQL.AdapterLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.AdapterRequest), PostgreSQL.AdapterRequestConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.AdapterRequestPayload), PostgreSQL.AdapterRequestPayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.AdapterResponse), PostgreSQL.AdapterResponseConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.AdapterResponsePayload), PostgreSQL.AdapterResponsePayloadConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.AdapterStatus), PostgreSQL.AdapterStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.Job), PostgreSQL.JobConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.JobData), PostgreSQL.JobDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.JobLog), PostgreSQL.JobLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.JobStatus), PostgreSQL.JobStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Components.Model.JobType), PostgreSQL.JobTypeConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.ESB.Components.Model.Adapter> Adapter { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.AdapterLog> AdapterLog { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.AdapterRequest> AdapterRequest { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.AdapterRequestPayload> AdapterRequestPayload { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.AdapterResponse> AdapterResponse { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.AdapterResponsePayload> AdapterResponsePayload { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.AdapterStatus> AdapterStatus { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.Job> Job { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.JobData> JobData { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.JobLog> JobLog { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.JobStatus> JobStatus { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Components.Model.JobType> JobType { get; set; }

	public ComponentsDbContext(DbContextOptions<ComponentsDbContext> options, Microsoft.Extensions.Logging.ILogger<ComponentsDbContext> logger)
		: base(options, logger)
	{
	}

	public ComponentsDbContext(Microsoft.Extensions.Logging.ILogger<ComponentsDbContext> logger)
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
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.AdapterConfiguration.Build(modelBuilder);
		PostgreSQL.AdapterLogConfiguration.Build(modelBuilder);
		PostgreSQL.AdapterRequestConfiguration.Build(modelBuilder);
		PostgreSQL.AdapterRequestPayloadConfiguration.Build(modelBuilder);
		PostgreSQL.AdapterResponseConfiguration.Build(modelBuilder);
		PostgreSQL.AdapterResponsePayloadConfiguration.Build(modelBuilder);
		PostgreSQL.AdapterStatusConfiguration.Build(modelBuilder);
		PostgreSQL.JobConfiguration.Build(modelBuilder);
		PostgreSQL.JobDataConfiguration.Build(modelBuilder);
		PostgreSQL.JobLogConfiguration.Build(modelBuilder);
		PostgreSQL.JobStatusConfiguration.Build(modelBuilder);
		PostgreSQL.JobTypeConfiguration.Build(modelBuilder);
	}
}
