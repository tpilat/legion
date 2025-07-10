using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.SqlServer;

public partial class ServiceBusDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ServiceBus.SqlServer.IServiceBusDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static ServiceBusDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ServiceBus.Model.Host), SqlServer.HostConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.HostLog), SqlServer.HostLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.Job), SqlServer.JobConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobData), SqlServer.JobDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobExecution), SqlServer.JobExecutionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobLog), SqlServer.JobLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobMessage), SqlServer.JobMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobMessageType), SqlServer.JobMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobRunType), SqlServer.JobRunTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobStatistics), SqlServer.JobStatisticsConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.JobStatus), SqlServer.JobStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.Orchestration), SqlServer.OrchestrationConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationInstance), SqlServer.OrchestrationInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStatus), SqlServer.OrchestrationStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStep), SqlServer.OrchestrationStepConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing), SqlServer.OrchestrationStepProcessingConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingDirection), SqlServer.OrchestrationStepProcessingDirectionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog), SqlServer.OrchestrationStepProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage), SqlServer.OrchestrationStepProcessingMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType), SqlServer.OrchestrationStepProcessingMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus), SqlServer.OrchestrationStepProcessingStatusConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.HostConfiguration.Build(modelBuilder);
		SqlServer.HostLogConfiguration.Build(modelBuilder);
		SqlServer.JobConfiguration.Build(modelBuilder);
		SqlServer.JobDataConfiguration.Build(modelBuilder);
		SqlServer.JobExecutionConfiguration.Build(modelBuilder);
		SqlServer.JobLogConfiguration.Build(modelBuilder);
		SqlServer.JobMessageConfiguration.Build(modelBuilder);
		SqlServer.JobMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.JobRunTypeConfiguration.Build(modelBuilder);
		SqlServer.JobStatisticsConfiguration.Build(modelBuilder);
		SqlServer.JobStatusConfiguration.Build(modelBuilder);
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
