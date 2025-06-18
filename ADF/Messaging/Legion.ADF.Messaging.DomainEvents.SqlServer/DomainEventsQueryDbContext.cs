using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public partial class DomainEventsQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsQueryDbContext
{
	public virtual DbSet<Legion.ADF.Messaging.DomainEvents.Model.VwDomainEvent> VwDomainEvent { get; set; }

	public DomainEventsQueryDbContext(DbContextOptions<DomainEventsQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<DomainEventsQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public DomainEventsQueryDbContext(Microsoft.Extensions.Logging.ILogger<DomainEventsQueryDbContext> logger)
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

		SqlServer.VwDomainEventConfiguration.Build(modelBuilder);
	}
}
