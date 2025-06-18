using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ESB.MBox.PostgreSQL;

public partial class MBoxQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.ESB.MBox.PostgreSQL.IMBoxQueryDbContext
{
	public virtual DbSet<Legion.ADF.ESB.MBox.Model.VwQueuedMessage> VwQueuedMessage { get; set; }

	public MBoxQueryDbContext(DbContextOptions<MBoxQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<MBoxQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public MBoxQueryDbContext(Microsoft.Extensions.Logging.ILogger<MBoxQueryDbContext> logger)
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

		PostgreSQL.VwQueuedMessageConfiguration.Build(modelBuilder);
	}
}
