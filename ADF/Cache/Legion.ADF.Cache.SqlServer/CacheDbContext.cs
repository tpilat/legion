using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Cache.SqlServer;

public partial class CacheDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Cache.SqlServer.ICacheDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static CacheDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Cache.Model.CacheData), SqlServer.CacheDataConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Cache.Model.DistributedLock), SqlServer.DistributedLockConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Cache.Model.ReloadableCacheKey), SqlServer.ReloadableCacheKeyConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Cache.Model.CacheData> CacheData { get; set; }
	public virtual DbSet<Legion.ADF.Cache.Model.DistributedLock> DistributedLock { get; set; }
	public virtual DbSet<Legion.ADF.Cache.Model.ReloadableCacheKey> ReloadableCacheKey { get; set; }

	public CacheDbContext(DbContextOptions<CacheDbContext> options, Microsoft.Extensions.Logging.ILogger<CacheDbContext> logger)
		: base(options, logger)
	{
	}

	public CacheDbContext(Microsoft.Extensions.Logging.ILogger<CacheDbContext> logger)
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

		SqlServer.CacheDataConfiguration.Build(modelBuilder);
		SqlServer.DistributedLockConfiguration.Build(modelBuilder);
		SqlServer.ReloadableCacheKeyConfiguration.Build(modelBuilder);
	}
}
