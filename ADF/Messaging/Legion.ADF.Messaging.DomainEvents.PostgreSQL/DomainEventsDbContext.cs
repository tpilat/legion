using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.DomainEvents.PostgreSQL;

public partial class DomainEventsDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.DomainEvents.PostgreSQL.IDomainEventsDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static DomainEventsDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.BlockedDomainEventType), PostgreSQL.BlockedDomainEventTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEvent), PostgreSQL.DomainEventConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventContent), PostgreSQL.DomainEventContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingLog), PostgreSQL.DomainEventProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.DomainEvents.Model.DomainEventProcessingStatus), PostgreSQL.DomainEventProcessingStatusConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_PostgreSql());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		PostgreSQL.BlockedDomainEventTypeConfiguration.Build(modelBuilder);
		PostgreSQL.DomainEventConfiguration.Build(modelBuilder);
		PostgreSQL.DomainEventContentConfiguration.Build(modelBuilder);
		PostgreSQL.DomainEventProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.DomainEventProcessingStatusConfiguration.Build(modelBuilder);
	}
}
