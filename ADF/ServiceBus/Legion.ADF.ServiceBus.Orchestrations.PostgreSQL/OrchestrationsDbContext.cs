using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public partial class OrchestrationsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static OrchestrationsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration), PostgreSQL.OrchestrationConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance), PostgreSQL.OrchestrationInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus), PostgreSQL.OrchestrationStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep), PostgreSQL.OrchestrationStepConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing), PostgreSQL.OrchestrationStepProcessingConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection), PostgreSQL.OrchestrationStepProcessingDirectionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog), PostgreSQL.OrchestrationStepProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage), PostgreSQL.OrchestrationStepProcessingMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType), PostgreSQL.OrchestrationStepProcessingMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus), PostgreSQL.OrchestrationStepProcessingStatusConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration> Orchestration { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance> OrchestrationInstance { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus> OrchestrationStatus { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep> OrchestrationStep { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing> OrchestrationStepProcessing { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection> OrchestrationStepProcessingDirection { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog> OrchestrationStepProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage> OrchestrationStepProcessingMessage { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType> OrchestrationStepProcessingMessageType { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus> OrchestrationStepProcessingStatus { get; set; }

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

		if (DbContextSettintgs.AllowLocking == true)
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_PostgreSql());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.OrchestrationConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationInstanceConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStatusConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepProcessingConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepProcessingDirectionConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepProcessingMessageConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepProcessingMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.OrchestrationStepProcessingStatusConfiguration.Build(modelBuilder);
	}
}
