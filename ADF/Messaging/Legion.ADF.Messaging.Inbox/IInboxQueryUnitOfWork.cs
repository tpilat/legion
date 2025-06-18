namespace Legion.ADF.Messaging.Inbox;

public partial interface IInboxQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Messaging.Inbox.Model.Repositories.IVwBlockedInboxMessageTypeRepository VwBlockedInboxMessageTypeRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageRepository VwInboxMessageRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageArchiveRepository VwInboxMessageArchiveRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageContentRepository VwInboxMessageContentRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxMessageProcessingLogRepository VwInboxMessageProcessingLogRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxQueueRepository VwInboxQueueRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IVwInboxQueueMessagesRepository VwInboxQueueMessagesRepository { get; }
}
