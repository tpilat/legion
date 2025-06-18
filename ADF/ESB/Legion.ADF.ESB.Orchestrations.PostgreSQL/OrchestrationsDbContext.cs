using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ESB.Orchestrations.PostgreSQL;

public partial class OrchestrationsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ESB.Orchestrations.PostgreSQL.IOrchestrationsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static OrchestrationsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.Orchestration), PostgreSQL.OrchestrationConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance), PostgreSQL.OrchestrationInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.OrchestrationStatus), PostgreSQL.OrchestrationStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep), PostgreSQL.OrchestrationStepConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepInstance), PostgreSQL.OrchestrationStepInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog), PostgreSQL.OrchestrationStepLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepStatus), PostgreSQL.OrchestrationStepStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ESB.Orchestrations.Model.StepDirection), PostgreSQL.StepDirectionConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.Orchestration> Orchestration { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationInstance> OrchestrationInstance { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStatus> OrchestrationStatus { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStep> OrchestrationStep { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepInstance> OrchestrationStepInstance { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepLog> OrchestrationStepLog { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.OrchestrationStepStatus> OrchestrationStepStatus { get; set; }
	public virtual DbSet<Legion.ADF.ESB.Orchestrations.Model.StepDirection> StepDirection { get; set; }

	public OrchestrationsDbContext(DbContextOptions<OrchestrationsDbContext> options, Microsoft.Extensions.Logging.ILogger<OrchestrationsDbContext> logger)
		: base(options, logger)
	{
	}

	public OrchestrationsDbContext(Microsoft.Extensions.Logging.ILogger<OrchestrationsDbContext> logger)
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

		PostgreSQL.OrchestrationConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationInstanceConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStatusConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepInstanceConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepLogConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepStatusConfiguration.Build(modelBuilder);
		PostgreSQL.StepDirectionConfiguration.Build(modelBuilder);
	}
}
