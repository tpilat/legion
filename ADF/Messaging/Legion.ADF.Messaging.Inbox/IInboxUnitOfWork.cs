using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Messaging.Inbox;

public partial interface IInboxUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Messaging.Inbox.Model.Repositories.IBlockedInboxMessageTypeRepository BlockedInboxMessageTypeRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxInstanceRepository InboxInstanceRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxMessageRepository InboxMessageRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxMessageArchiveRepository InboxMessageArchiveRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxMessageContentRepository InboxMessageContentRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxMessageProcessingLogRepository InboxMessageProcessingLogRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxMessageStatusRepository InboxMessageStatusRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxMessageTypeRepository InboxMessageTypeRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxProcessingLogRepository InboxProcessingLogRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxQueueRepository InboxQueueRepository { get; }

	Legion.ADF.Messaging.Inbox.Model.Repositories.IInboxQueueProcessingModeRepository InboxQueueProcessingModeRepository { get; }
}
