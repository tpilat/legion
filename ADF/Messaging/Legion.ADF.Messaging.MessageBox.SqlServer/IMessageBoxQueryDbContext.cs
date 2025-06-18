using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public interface IMessageBoxQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType> VwBlockedMessageType { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessage> VwMessage { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive> VwMessageArchive { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent> VwMessageContent { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog> VwMessageProcessingLog { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwQueue> VwQueue { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage> VwQueuedMessage { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages> VwQueueMessages { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage> VwSubscribedMessage { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwTopic> VwTopic { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription> VwTopicSubscription { get; set; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages> VwTopicSubscriptionMessages { get; set; }
}
