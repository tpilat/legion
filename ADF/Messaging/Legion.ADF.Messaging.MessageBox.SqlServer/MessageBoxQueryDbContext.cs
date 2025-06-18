using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public partial class MessageBoxQueryDbContext : Legion.EntityFrameworkCore.DbContextBase, Legion.ADF.Messaging.MessageBox.SqlServer.IMessageBoxQueryDbContext
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

		SqlServer.VwBlockedMessageTypeConfiguration.Build(modelBuilder);
		SqlServer.VwMessageConfiguration.Build(modelBuilder);
		SqlServer.VwMessageArchiveConfiguration.Build(modelBuilder);
		SqlServer.VwMessageContentConfiguration.Build(modelBuilder);
		SqlServer.VwMessageProcessingLogConfiguration.Build(modelBuilder);
		SqlServer.VwQueueConfiguration.Build(modelBuilder);
		SqlServer.VwQueuedMessageConfiguration.Build(modelBuilder);
		SqlServer.VwQueueMessagesConfiguration.Build(modelBuilder);
		SqlServer.VwSubscribedMessageConfiguration.Build(modelBuilder);
		SqlServer.VwTopicConfiguration.Build(modelBuilder);
		SqlServer.VwTopicSubscriptionConfiguration.Build(modelBuilder);
		SqlServer.VwTopicSubscriptionMessagesConfiguration.Build(modelBuilder);
	}
}
