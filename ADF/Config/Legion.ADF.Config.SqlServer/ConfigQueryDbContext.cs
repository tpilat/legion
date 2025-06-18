using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Config.SqlServer;

public partial class ConfigQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Config.SqlServer.IConfigQueryDbContext
{
	public virtual DbSet<Legion.ADF.Config.Model.VwConfigurationClass> VwConfigurationClass { get; set; }

	public ConfigQueryDbContext(DbContextOptions<ConfigQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<ConfigQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public ConfigQueryDbContext(Microsoft.Extensions.Logging.ILogger<ConfigQueryDbContext> logger)
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

		SqlServer.VwConfigurationClassConfiguration.Build(modelBuilder);
	}
}
