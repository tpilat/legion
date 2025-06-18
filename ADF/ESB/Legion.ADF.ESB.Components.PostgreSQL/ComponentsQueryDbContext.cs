using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ESB.Components.PostgreSQL;

public partial class ComponentsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.ESB.Components.PostgreSQL.IComponentsQueryDbContext
{
	public virtual DbSet<Legion.ADF.ESB.Components.Model.VwJob> VwJob { get; set; }

	public ComponentsQueryDbContext(DbContextOptions<ComponentsQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<ComponentsQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public ComponentsQueryDbContext(Microsoft.Extensions.Logging.ILogger<ComponentsQueryDbContext> logger)
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
