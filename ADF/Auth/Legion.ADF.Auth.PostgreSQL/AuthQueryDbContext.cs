using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Auth.PostgreSQL;

public partial class AuthQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Auth.PostgreSQL.IAuthQueryDbContext
{
	public virtual DbSet<Legion.ADF.Auth.Model.VwUser> VwUser { get; set; }

	public AuthQueryDbContext(DbContextOptions<AuthQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<AuthQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public AuthQueryDbContext(Microsoft.Extensions.Logging.ILogger<AuthQueryDbContext> logger)
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

		PostgreSQL.VwUserConfiguration.Build(modelBuilder);
	}
}
