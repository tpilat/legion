using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public partial class JobsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static JobsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.Job), PostgreSQL.JobConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData), PostgreSQL.JobDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution), PostgreSQL.JobExecutionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog), PostgreSQL.JobLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage), PostgreSQL.JobMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessageType), PostgreSQL.JobMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobRunType), PostgreSQL.JobRunTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics), PostgreSQL.JobStatisticsConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatus), PostgreSQL.JobStatusConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.Job> Job { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobData> JobData { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobExecution> JobExecution { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobLog> JobLog { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobMessage> JobMessage { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType> JobMessageType { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobRunType> JobRunType { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobStatistics> JobStatistics { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.JobStatus> JobStatus { get; set; }

	public JobsDbContext(DbContextOptions<JobsDbContext> options, Microsoft.Extensions.Logging.ILogger<JobsDbContext> logger)
		: base(options, logger)
	{
	}

	public JobsDbContext(Microsoft.Extensions.Logging.ILogger<JobsDbContext> logger)
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

		PostgreSQL.JobConfiguration.Build(modelBuilder);
		PostgreSQL.JobDataConfiguration.Build(modelBuilder);
		PostgreSQL.JobExecutionConfiguration.Build(modelBuilder);
		PostgreSQL.JobLogConfiguration.Build(modelBuilder);
		PostgreSQL.JobMessageConfiguration.Build(modelBuilder);
		PostgreSQL.JobMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.JobRunTypeConfiguration.Build(modelBuilder);
		PostgreSQL.JobStatisticsConfiguration.Build(modelBuilder);
		PostgreSQL.JobStatusConfiguration.Build(modelBuilder);
	}
}
