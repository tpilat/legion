using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.PostgreSQL;

public partial class ServiceBusQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.ServiceBus.PostgreSQL.IServiceBusQueryDbContext
{
	public virtual DbSet<Legion.ADF.ServiceBus.Model.VwHost> VwHost { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.VwJob> VwJob { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Model.VwOrchestration> VwOrchestration { get; set; }

	public ServiceBusQueryDbContext(DbContextOptions<ServiceBusQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<ServiceBusQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public ServiceBusQueryDbContext(Microsoft.Extensions.Logging.ILogger<ServiceBusQueryDbContext> logger)
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

		PostgreSQL.VwHostConfiguration.Build(modelBuilder);
		PostgreSQL.VwJobConfiguration.Build(modelBuilder);
		PostgreSQL.VwOrchestrationConfiguration.Build(modelBuilder);
	}
}
