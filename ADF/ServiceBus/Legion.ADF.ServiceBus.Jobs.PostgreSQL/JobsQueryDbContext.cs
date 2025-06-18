using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Jobs.PostgreSQL;

public partial class JobsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.ServiceBus.Jobs.PostgreSQL.IJobsQueryDbContext
{
	public virtual DbSet<Legion.ADF.ServiceBus.Jobs.Model.VwJob> VwJob { get; set; }

	public JobsQueryDbContext(DbContextOptions<JobsQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<JobsQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public JobsQueryDbContext(Microsoft.Extensions.Logging.ILogger<JobsQueryDbContext> logger)
		: base(logger)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		
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

		PostgreSQL.VwJobConfiguration.Build(modelBuilder);
	}
}
