using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public partial class OutboxDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.Outbox.SqlServer.IOutboxDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static OutboxDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType), SqlServer.BlockedOutboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxInstance), SqlServer.OutboxInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessage), SqlServer.OutboxMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive), SqlServer.OutboxMessageArchiveConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent), SqlServer.OutboxMessageContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog), SqlServer.OutboxMessageProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus), SqlServer.OutboxMessageStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxMessageType), SqlServer.OutboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog), SqlServer.OutboxProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueue), SqlServer.OutboxQueueConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode), SqlServer.OutboxQueueProcessingModeConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.BlockedOutboxMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.OutboxInstanceConfiguration.Build(modelBuilder);
		SqlServer.OutboxMessageConfiguration.Build(modelBuilder);
		SqlServer.OutboxMessageArchiveConfiguration.Build(modelBuilder);
		SqlServer.OutboxMessageContentConfiguration.Build(modelBuilder);
		SqlServer.OutboxMessageProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.OutboxMessageStatusConfiguration.Build(modelBuilder);
		SqlServer.OutboxMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.OutboxProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.OutboxQueueConfiguration.Build(modelBuilder);
		SqlServer.OutboxQueueProcessingModeConfiguration.Build(modelBuilder);
	}
}
