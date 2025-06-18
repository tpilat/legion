using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public partial class InboxDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.Inbox.SqlServer.IInboxDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static InboxDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType), SqlServer.BlockedInboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxInstance), SqlServer.InboxInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessage), SqlServer.InboxMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive), SqlServer.InboxMessageArchiveConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageContent), SqlServer.InboxMessageContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog), SqlServer.InboxMessageProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus), SqlServer.InboxMessageStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxMessageType), SqlServer.InboxMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog), SqlServer.InboxProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueue), SqlServer.InboxQueueConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode), SqlServer.InboxQueueProcessingModeConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.BlockedInboxMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.InboxInstanceConfiguration.Build(modelBuilder);
		SqlServer.InboxMessageConfiguration.Build(modelBuilder);
		SqlServer.InboxMessageArchiveConfiguration.Build(modelBuilder);
		SqlServer.InboxMessageContentConfiguration.Build(modelBuilder);
		SqlServer.InboxMessageProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.InboxMessageStatusConfiguration.Build(modelBuilder);
		SqlServer.InboxMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.InboxProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.InboxQueueConfiguration.Build(modelBuilder);
		SqlServer.InboxQueueProcessingModeConfiguration.Build(modelBuilder);
	}
}
