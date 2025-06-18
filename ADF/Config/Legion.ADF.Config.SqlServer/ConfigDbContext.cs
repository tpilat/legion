using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Config.SqlServer;

public partial class ConfigDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Config.SqlServer.IConfigDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static ConfigDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Config.Model.ConfigurationClass), SqlServer.ConfigurationClassConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Config.Model.ConfigurationKeyValue), SqlServer.ConfigurationKeyValueConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Config.Model.ConfigurationClass> ConfigurationClass { get; set; }
	public virtual DbSet<Legion.ADF.Config.Model.ConfigurationKeyValue> ConfigurationKeyValue { get; set; }

	public ConfigDbContext(DbContextOptions<ConfigDbContext> options, Microsoft.Extensions.Logging.ILogger<ConfigDbContext> logger)
		: base(options, logger)
	{
	}

	public ConfigDbContext(Microsoft.Extensions.Logging.ILogger<ConfigDbContext> logger)
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

		SqlServer.ConfigurationClassConfiguration.Build(modelBuilder);
		SqlServer.ConfigurationKeyValueConfiguration.Build(modelBuilder);
	}
}
