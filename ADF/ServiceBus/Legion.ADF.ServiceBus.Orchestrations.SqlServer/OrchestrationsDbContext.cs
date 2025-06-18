using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Orchestrations.SqlServer;

public partial class OrchestrationsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ServiceBus.Orchestrations.SqlServer.IOrchestrationsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static OrchestrationsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration), SqlServer.OrchestrationConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationInstance), SqlServer.OrchestrationInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus), SqlServer.OrchestrationStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep), SqlServer.OrchestrationStepConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing), SqlServer.OrchestrationStepProcessingConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingDirection), SqlServer.OrchestrationStepProcessingDirectionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingLog), SqlServer.OrchestrationStepProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessage), SqlServer.OrchestrationStepProcessingMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingMessageType), SqlServer.OrchestrationStepProcessingMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus), SqlServer.OrchestrationStepProcessingStatusConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.OrchestrationConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationInstanceConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStatusConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStepConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStepProcessingConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStepProcessingDirectionConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStepProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStepProcessingMessageConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStepProcessingMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.OrchestrationStepProcessingStatusConfiguration.Build(modelBuilder);
	}
}
