using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Cache.PostgreSQL;

public partial class CacheQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Cache.PostgreSQL.ICacheQueryDbContext
{
	public virtual DbSet<Legion.ADF.Cache.Model.VwReloadableCacheKey> VwReloadableCacheKey { get; set; }

	public CacheQueryDbContext(DbContextOptions<CacheQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<CacheQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public CacheQueryDbContext(Microsoft.Extensions.Logging.ILogger<CacheQueryDbContext> logger)
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

		PostgreSQL.VwReloadableCacheKeyConfiguration.Build(modelBuilder);
	}
}
