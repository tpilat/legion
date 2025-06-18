using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Inbox.SqlServer;

public interface IInboxQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType> VwBlockedInboxMessageType { get; set; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage> VwInboxMessage { get; set; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive> VwInboxMessageArchive { get; set; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent> VwInboxMessageContent { get; set; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog> VwInboxMessageProcessingLog { get; set; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxQueue> VwInboxQueue { get; set; }
	DbSet<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages> VwInboxQueueMessages { get; set; }
}
