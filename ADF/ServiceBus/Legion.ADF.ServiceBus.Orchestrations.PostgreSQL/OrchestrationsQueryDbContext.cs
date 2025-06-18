using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Orchestrations.PostgreSQL;

public partial class OrchestrationsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.ServiceBus.Orchestrations.PostgreSQL.IOrchestrationsQueryDbContext
{
	public virtual DbSet<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration> VwOrchestration { get; set; }

	public OrchestrationsQueryDbContext(DbContextOptions<OrchestrationsQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<OrchestrationsQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public OrchestrationsQueryDbContext(Microsoft.Extensions.Logging.ILogger<OrchestrationsQueryDbContext> logger)
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

		PostgreSQL.VwOrchestrationConfiguration.Build(modelBuilder);
	}
}
