using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Hosts.SqlServer;

public partial class HostsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.ServiceBus.Hosts.SqlServer.IHostsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static HostsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.ServiceBus.Hosts.Model.Host), SqlServer.HostConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.ServiceBus.Hosts.Model.HostLog), SqlServer.HostLogConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.ServiceBus.Hosts.Model.Host> Host { get; set; }
	public virtual DbSet<Legion.ADF.ServiceBus.Hosts.Model.HostLog> HostLog { get; set; }

	public HostsDbContext(DbContextOptions<HostsDbContext> options, Microsoft.Extensions.Logging.ILogger<HostsDbContext> logger)
		: base(options, logger)
	{
	}

	public HostsDbContext(Microsoft.Extensions.Logging.ILogger<HostsDbContext> logger)
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

		SqlServer.HostConfiguration.Build(modelBuilder);
		SqlServer.HostLogConfiguration.Build(modelBuilder);
	}
}
