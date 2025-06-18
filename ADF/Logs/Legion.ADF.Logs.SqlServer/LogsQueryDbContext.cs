using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Logs.SqlServer;

public partial class LogsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Logs.SqlServer.ILogsQueryDbContext
{
	public virtual DbSet<Legion.ADF.Logs.Model.VwLog> VwLog { get; set; }

	public LogsQueryDbContext(DbContextOptions<LogsQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<LogsQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public LogsQueryDbContext(Microsoft.Extensions.Logging.ILogger<LogsQueryDbContext> logger)
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

		SqlServer.VwLogConfiguration.Build(modelBuilder);
	}
}
