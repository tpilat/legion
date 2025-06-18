using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Jobs.SqlServer;

public partial class JobsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ServiceBus.Jobs.SqlServer.IJobsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static JobsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.Job), SqlServer.JobConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobData), SqlServer.JobDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobExecution), SqlServer.JobExecutionConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobLog), SqlServer.JobLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessage), SqlServer.JobMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobMessageType), SqlServer.JobMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobRunType), SqlServer.JobRunTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatistics), SqlServer.JobStatisticsConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Jobs.Model.JobStatus), SqlServer.JobStatusConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.JobConfiguration.Build(modelBuilder);
		SqlServer.JobDataConfiguration.Build(modelBuilder);
		SqlServer.JobExecutionConfiguration.Build(modelBuilder);
		SqlServer.JobLogConfiguration.Build(modelBuilder);
		SqlServer.JobMessageConfiguration.Build(modelBuilder);
		SqlServer.JobMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.JobRunTypeConfiguration.Build(modelBuilder);
		SqlServer.JobStatisticsConfiguration.Build(modelBuilder);
		SqlServer.JobStatusConfiguration.Build(modelBuilder);
	}
}
