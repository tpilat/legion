namespace Legion.ADF.Messaging.Outbox;

public partial interface IOutboxQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Messaging.Outbox.Model.Repositories.IVwBlockedOutboxMessageTypeRepository VwBlockedOutboxMessageTypeRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageRepository VwOutboxMessageRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageArchiveRepository VwOutboxMessageArchiveRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageContentRepository VwOutboxMessageContentRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxMessageProcessingLogRepository VwOutboxMessageProcessingLogRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueRepository VwOutboxQueueRepository { get; }

	Legion.ADF.Messaging.Outbox.Model.Repositories.IVwOutboxQueueMessagesRepository VwOutboxQueueMessagesRepository { get; }
}
