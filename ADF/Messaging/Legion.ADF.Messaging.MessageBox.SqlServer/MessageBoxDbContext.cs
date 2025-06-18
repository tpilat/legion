using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public partial class MessageBoxDbContext : Legion.EntityFrameworkCore.Audit.AuditableDbContext, Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxDbContext
{
	public static readonly System.Collections.Generic.IReadOnlyDictionary<string, string> PrimaryKeyFormatters;

	static MessageBoxDbContext()
	{
		PrimaryKeyFormatters = new System.Collections.Generic.Dictionary<string, string>
		{
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType), SqlServer.BlockedMessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.Message), SqlServer.MessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageArchive), SqlServer.MessageArchiveConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance), SqlServer.MessageBoxInstanceConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog), SqlServer.MessageBoxProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageContent), SqlServer.MessageContentConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog), SqlServer.MessageProcessingLogConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus), SqlServer.MessageProcessingStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageStatus), SqlServer.MessageStatusConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.MessageType), SqlServer.MessageTypeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.Queue), SqlServer.QueueConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.QueuedMessage), SqlServer.QueuedMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode), SqlServer.QueueProcessingModeConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage), SqlServer.SubscribedMessageConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.Topic), SqlServer.TopicConfiguration.PrimaryKeyFormatter },
			{ nameof(Legion.ADF.Messaging.MessageBox.Model.TopicSubscription), SqlServer.TopicSubscriptionConfiguration.PrimaryKeyFormatter },
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
			optionsBuilder.AddInterceptors(new Legion.EntityFrameworkCore.Interceptors.RowLockInterceptor_SqlServer());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		RegisterUnaccentFunction(modelBuilder);

		SqlServer.BlockedMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.MessageConfiguration.Build(modelBuilder);
		SqlServer.MessageArchiveConfiguration.Build(modelBuilder);
		SqlServer.MessageBoxInstanceConfiguration.Build(modelBuilder);
		SqlServer.MessageBoxProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.MessageContentConfiguration.Build(modelBuilder);
		SqlServer.MessageProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.MessageProcessingStatusConfiguration.Build(modelBuilder);
		SqlServer.MessageStatusConfiguration.Build(modelBuilder);
		SqlServer.MessageTypeConfiguration.Build(modelBuilder);
		SqlServer.QueueConfiguration.Build(modelBuilder);
		SqlServer.QueuedMessageConfiguration.Build(modelBuilder);
		SqlServer.QueueProcessingModeConfiguration.Build(modelBuilder);
		SqlServer.SubscribedMessageConfiguration.Build(modelBuilder);
		SqlServer.TopicConfiguration.Build(modelBuilder);
		SqlServer.TopicSubscriptionConfiguration.Build(modelBuilder);
	}
}
