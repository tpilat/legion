using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Legion.ADF.Messaging.Outbox.SqlServer;

public interface IOutboxDbContext : Legion.EntityFrameworkCore.Audit.IAuditableDbContext, Legion.EntityFrameworkCore.IDbContext, IDisposable, IAsyncDisposable
{
	DbSet<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType> BlockedOutboxMessageType { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxInstance> OutboxInstance { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessage> OutboxMessage { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive> OutboxMessageArchive { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent> OutboxMessageContent { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog> OutboxMessageProcessingLog { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus> OutboxMessageStatus { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType> OutboxMessageType { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog> OutboxProcessingLog { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxQueue> OutboxQueue { get; }
	DbSet<Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode> OutboxQueueProcessingMode { get; }
}
