using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.MessageBox.PostgreSQL;

public partial class MessageBoxQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Messaging.MessageBox.PostgreSQL.IMessageBoxQueryDbContext
{
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> VwBlockedMessageType { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessage> VwMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> VwMessageArchive { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent> VwMessageContent { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> VwMessageProcessingLog { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwQueue> VwQueue { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage> VwQueuedMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> VwQueueMessages { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage> VwSubscribedMessage { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwTopic> VwTopic { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription> VwTopicSubscription { get; set; }
	public virtual DbSet<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> VwTopicSubscriptionMessages { get; set; }

	public MessageBoxQueryDbContext(DbContextOptions<MessageBoxQueryDbContext> options, Microsoft.Extensions.Logging.ILogger<MessageBoxQueryDbContext> logger)
		: base(options, logger)
	{
	}

	public MessageBoxQueryDbContext(Microsoft.Extensions.Logging.ILogger<MessageBoxQueryDbContext> logger)
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

		PostgreSQL.VwBlockedMessageTypeConfiguration.Build(modelBuilder);
		PostgreSQL.VwMessageConfiguration.Build(modelBuilder);
		PostgreSQL.VwMessageArchiveConfiguration.Build(modelBuilder);
		PostgreSQL.VwMessageContentConfiguration.Build(modelBuilder);
		PostgreSQL.VwMessageProcessingLogConfiguration.Build(modelBuilder);
		PostgreSQL.VwQueueConfiguration.Build(modelBuilder);
		PostgreSQL.VwQueuedMessageConfiguration.Build(modelBuilder);
		PostgreSQL.VwQueueMessagesConfiguration.Build(modelBuilder);
		PostgreSQL.VwSubscribedMessageConfiguration.Build(modelBuilder);
		PostgreSQL.VwTopicConfiguration.Build(modelBuilder);
		PostgreSQL.VwTopicSubscriptionConfiguration.Build(modelBuilder);
		PostgreSQL.VwTopicSubscriptionMessagesConfiguration.Build(modelBuilder);
	}
}
