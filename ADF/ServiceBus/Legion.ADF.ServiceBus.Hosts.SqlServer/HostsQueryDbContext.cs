using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.ServiceBus.Hosts.SqlServer;

public partial class HostsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.ServiceBus.Hosts.SqlServer.IHostsQueryDbContext
{
	public virtual DbSet<Legion.ADF.ServiceBus.Hosts.Model.VwHost> VwHost { get; set; }

	public HostsQueryDbContext(DbContextOptions<HostsQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<HostsQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public HostsQueryDbContext(Microsoft.Extensions.Logging.ILogger<HostsQueryDbContext> logger)
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

		SqlServer.VwHostConfiguration.Build(modelBuilder);
	}
}
