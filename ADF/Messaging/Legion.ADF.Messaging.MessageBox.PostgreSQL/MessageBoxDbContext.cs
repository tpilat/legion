using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public partial class MessageBoxDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static MessageBoxDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType), PostgreSQL.BlockedMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.Message), PostgreSQL.MessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive), PostgreSQL.MessageArchiveConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance), PostgreSQL.MessageBoxInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog), PostgreSQL.MessageBoxProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent), PostgreSQL.MessageContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog), PostgreSQL.MessageProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus), PostgreSQL.MessageProcessingStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageStatus), PostgreSQL.MessageStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType), PostgreSQL.MessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.Queue), PostgreSQL.QueueConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage), PostgreSQL.QueuedMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode), PostgreSQL.QueueProcessingModeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage), PostgreSQL.SubscribedMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.Topic), PostgreSQL.TopicConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription), PostgreSQL.TopicSubscriptionConfiguration.PrimaryKeyFormatter },
		};
	}

	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType> BlockedMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.Message> Message { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageArchive> MessageArchive { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance> MessageBoxInstance { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog> MessageBoxProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageContent> MessageContent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> MessageProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus> MessageProcessingStatus { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageStatus> MessageStatus { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageType> MessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.Queue> Queue { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> QueuedMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode> QueueProcessingMode { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> SubscribedMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.Topic> Topic { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> TopicSubscription { get; set; }

	public MessageBoxDbContext(DbContextOptions<MessageBoxDbContext> options, Microsoft.Extensions.Logging.ILogger<MessageBoxDbContext> logger)
		: base(options, logger)
	{
	}

	public MessageBoxDbContext(Microsoft.Extensions.Logging.ILogger<MessageBoxDbContext> logger)
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

		PostgreSQL.BlockedMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.MessageConfiguration.Build(modelBuilder);
		PostgreSQL.MessageArchiveConfiguration.Build(modelBuilder);
		PostgreSQL.MessageBoxInstanceConfiguration.Build(modelBuilder);
		PostgreSQL.MessageBoxProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.MessageContentConfiguration.Build(modelBuilder);
		PostgreSQL.MessageProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.MessageProcessingStatusConfiguration.Build(modelBuilder);
		PostgreSQL.MessageStatusConfiguration.Build(modelBuilder);
		PostgreSQL.MessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.QueueConfiguration.Build(modelBuilder);
		PostgreSQL.QueuedMessageConfiguration.Build(modelBuilder);
		PostgreSQL.QueueProcessingModeConfiguration.Build(modelBuilder);
		PostgreSQL.SubscribedMessageConfiguration.Build(modelBuilder);
		PostgreSQL.TopicConfiguration.Build(modelBuilder);
		PostgreSQL.TopicSubscriptionConfiguration.Build(modelBuilder);
	}
}
