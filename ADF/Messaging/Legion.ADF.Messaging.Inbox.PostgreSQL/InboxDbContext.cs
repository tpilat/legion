using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public partial class InboxDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.Inbox.PostgreSQL.IInboxDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static InboxDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType), PostgreSQL.BlockedInboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance), PostgreSQL.InboxInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage), PostgreSQL.InboxMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive), PostgreSQL.InboxMessageArchiveConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent), PostgreSQL.InboxMessageContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog), PostgreSQL.InboxMessageProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus), PostgreSQL.InboxMessageStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType), PostgreSQL.InboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog), PostgreSQL.InboxProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue), PostgreSQL.InboxQueueConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode), PostgreSQL.InboxQueueProcessingModeConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> BlockedInboxMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxInstance> InboxInstance { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessage> InboxMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive> InboxMessageArchive { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> InboxMessageContent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog> InboxMessageProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus> InboxMessageStatus { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageType> InboxMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog> InboxProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxQueue> InboxQueue { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode> InboxQueueProcessingMode { get; set; }

	public InboxDbContext(DbContextOptions<InboxDbContext> options, Microsoft.Extensions.Logging.ILogger<InboxDbContext> logger)
		: base(options, logger)
	{
	}

	public InboxDbContext(Microsoft.Extensions.Logging.ILogger<InboxDbContext> logger)
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

		PostgreSQL.BlockedInboxMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.InboxInstanceConfiguration.Build(modelBuilder);
		PostgreSQL.InboxMessageConfiguration.Build(modelBuilder);
		PostgreSQL.InboxMessageArchiveConfiguration.Build(modelBuilder);
		PostgreSQL.InboxMessageContentConfiguration.Build(modelBuilder);
		PostgreSQL.InboxMessageProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.InboxMessageStatusConfiguration.Build(modelBuilder);
		PostgreSQL.InboxMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.InboxProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.InboxQueueConfiguration.Build(modelBuilder);
		PostgreSQL.InboxQueueProcessingModeConfiguration.Build(modelBuilder);
	}
}
