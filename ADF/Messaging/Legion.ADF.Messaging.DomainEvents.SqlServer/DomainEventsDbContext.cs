using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.SqlServer;

public partial class DomainEventsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.DomainEvents.SqlServer.IDomainEventsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static DomainEventsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType), SqlServer.BlockedDomainEventTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent), SqlServer.DomainEventConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent), SqlServer.DomainEventContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog), SqlServer.DomainEventProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus), SqlServer.DomainEventProcessingStatusConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType> BlockedDomainEventType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEvent> DomainEvent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent> DomainEventContent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog> DomainEventProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus> DomainEventProcessingStatus { get; set; }

	public DomainEventsDbContext(DbContextOptions<DomainEventsDbContext> options, Microsoft.Extensions.Logging.ILogger<DomainEventsDbContext> logger)
		: base(options, logger)
	{
	}

	public DomainEventsDbContext(Microsoft.Extensions.Logging.ILogger<DomainEventsDbContext> logger)
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

		SqlServer.BlockedDomainEventTypeConfiguration.Build(modelBuilder);
		SqlServer.DomainEventConfiguration.Build(modelBuilder);
		SqlServer.DomainEventContentConfiguration.Build(modelBuilder);
		SqlServer.DomainEventProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.DomainEventProcessingStatusConfiguration.Build(modelBuilder);
	}
}
