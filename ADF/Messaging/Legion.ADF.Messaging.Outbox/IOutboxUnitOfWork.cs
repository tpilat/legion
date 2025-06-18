using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Messaging.Outbox;

public partial interface IOutboxUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Messaging.Outbox.Model.Repositories.IBlockedOutboxMessageTypeRepository BlockedOutboxMessageTypeRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxInstanceRepository OutboxInstanceRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxMessageRepository OutboxMessageRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxMessageArchiveRepository OutboxMessageArchiveRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxMessageContentRepository OutboxMessageContentRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxMessageProcessingLogRepository OutboxMessageProcessingLogRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxMessageStatusRepository OutboxMessageStatusRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxMessageTypeRepository OutboxMessageTypeRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxProcessingLogRepository OutboxProcessingLogRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxQueueRepository OutboxQueueRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IOutboxQueueProcessingModeRepository OutboxQueueProcessingModeRepository { get; }
}
