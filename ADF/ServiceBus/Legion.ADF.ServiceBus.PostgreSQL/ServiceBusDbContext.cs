using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.PostgreSQL;

public partial class ServiceBusDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ServiceBus.PostgreSQL.IServiceBusDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static ServiceBusDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ServiceBus.Model.Host), PostgreSQL.HostConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.HostLog), PostgreSQL.HostLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.Job), PostgreSQL.JobConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobData), PostgreSQL.JobDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobExecution), PostgreSQL.JobExecutionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobLog), PostgreSQL.JobLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobMessage), PostgreSQL.JobMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobMessageType), PostgreSQL.JobMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobRunType), PostgreSQL.JobRunTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobStatistics), PostgreSQL.JobStatisticsConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobStatus), PostgreSQL.JobStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.Orchestration), PostgreSQL.OrchestrationConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance), PostgreSQL.OrchestrationInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus), PostgreSQL.OrchestrationStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep), PostgreSQL.OrchestrationStepConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing), PostgreSQL.OrchestrationStepProcessingConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection), PostgreSQL.OrchestrationStepProcessingDirectionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog), PostgreSQL.OrchestrationStepProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage), PostgreSQL.OrchestrationStepProcessingMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType), PostgreSQL.OrchestrationStepProcessingMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus), PostgreSQL.OrchestrationStepProcessingStatusConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.ServiceBus.Model.Host> Host { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.HostLog> HostLog { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.Job> Job { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobData> JobData { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobExecution> JobExecution { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobLog> JobLog { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobMessage> JobMessage { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobMessageType> JobMessageType { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobRunType> JobRunType { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobStatistics> JobStatistics { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.JobStatus> JobStatus { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.Orchestration> Orchestration { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationInstance> OrchestrationInstance { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStatus> OrchestrationStatus { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStep> OrchestrationStep { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing> OrchestrationStepProcessing { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection> OrchestrationStepProcessingDirection { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog> OrchestrationStepProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage> OrchestrationStepProcessingMessage { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType> OrchestrationStepProcessingMessageType { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus> OrchestrationStepProcessingStatus { get; set; }

	public ServiceBusDbContext(DbContextOptions<ServiceBusDbContext> options, Microsoft.Extensions.Logging.ILogger<ServiceBusDbContext> logger)
		: base(options, logger)
	{
	}

	public ServiceBusDbContext(Microsoft.Extensions.Logging.ILogger<ServiceBusDbContext> logger)
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

		PostgreSQL.HostConfiguration.Build(modelBuilder);
		PostgreSQL.HostLogConfiguration.Build(modelBuilder);
		PostgreSQL.JobConfiguration.Build(modelBuilder);
		PostgreSQL.JobDataConfiguration.Build(modelBuilder);
		PostgreSQL.JobExecutionConfiguration.Build(modelBuilder);
		PostgreSQL.JobLogConfiguration.Build(modelBuilder);
		PostgreSQL.JobMessageConfiguration.Build(modelBuilder);
		PostgreSQL.JobMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.JobRunTypeConfiguration.Build(modelBuilder);
		PostgreSQL.JobStatisticsConfiguration.Build(modelBuilder);
		PostgreSQL.JobStatusConfiguration.Build(modelBuilder);
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
