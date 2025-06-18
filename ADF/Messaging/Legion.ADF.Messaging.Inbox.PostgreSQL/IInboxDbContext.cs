using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Messaging.Inbox.PostgreSQL;

public interface IInboxDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType> BlockedInboxMessageType { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxInstance> InboxInstance { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessage> InboxMessage { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive> InboxMessageArchive { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent> InboxMessageContent { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog> InboxMessageProcessingLog { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus> InboxMessageStatus { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxMessageType> InboxMessageType { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog> InboxProcessingLog { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxQueue> InboxQueue { get; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode> InboxQueueProcessingMode { get; }
}
