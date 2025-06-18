using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Messaging.MessageBox.SqlServer;

public interface IMessageBoxDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType> BlockedMessageType { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.Message> Message { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageArchive> MessageArchive { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance> MessageBoxInstance { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog> MessageBoxProcessingLog { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageContent> MessageContent { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog> MessageProcessingLog { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus> MessageProcessingStatus { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageStatus> MessageStatus { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.MessageType> MessageType { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.Queue> Queue { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage> QueuedMessage { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode> QueueProcessingMode { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage> SubscribedMessage { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.Topic> Topic { get; }
	DbSet<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription> TopicSubscription { get; }
}
