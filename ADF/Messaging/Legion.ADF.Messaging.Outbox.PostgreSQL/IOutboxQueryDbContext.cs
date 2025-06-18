using Microsoft.EntityFrameworkCore;

namespace Legion.ADF.Messaging.Outbox.PostgreSQL;

public interface IOutboxQueryDbContext : Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType> VwBlockedOutboxMessageType { get; set; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage> VwOutboxMessage { get; set; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive> VwOutboxMessageArchive { get; set; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent> VwOutboxMessageContent { get; set; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog> VwOutboxMessageProcessingLog { get; set; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue> VwOutboxQueue { get; set; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages> VwOutboxQueueMessages { get; set; }
}
