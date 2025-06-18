using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public partial class OutboxDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.Outbox.PostgreSQL.IOutboxDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static OutboxDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType), PostgreSQL.BlockedOutboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance), PostgreSQL.OutboxInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage), PostgreSQL.OutboxMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive), PostgreSQL.OutboxMessageArchiveConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent), PostgreSQL.OutboxMessageContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog), PostgreSQL.OutboxMessageProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus), PostgreSQL.OutboxMessageStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType), PostgreSQL.OutboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog), PostgreSQL.OutboxProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue), PostgreSQL.OutboxQueueConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode), PostgreSQL.OutboxQueueProcessingModeConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> BlockedOutboxMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxInstance> OutboxInstance { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessage> OutboxMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive> OutboxMessageArchive { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent> OutboxMessageContent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog> OutboxMessageProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus> OutboxMessageStatus { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> OutboxMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog> OutboxProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxQueue> OutboxQueue { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode> OutboxQueueProcessingMode { get; set; }

	public OutboxDbContext(DbContextOptions<OutboxDbContext> options, Microsoft.Extensions.Logging.ILogger<OutboxDbContext> logger)
		: base(options, logger)
	{
	}

	public OutboxDbContext(Microsoft.Extensions.Logging.ILogger<OutboxDbContext> logger)
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

		PostgreSQL.BlockedOutboxMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxInstanceConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxMessageConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxMessageArchiveConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxMessageContentConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxMessageProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxMessageStatusConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxQueueConfiguration.Build(modelBuilder);
		PostgreSQL.OutboxQueueProcessingModeConfiguration.Build(modelBuilder);
	}
}
