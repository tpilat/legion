using Legion.Database;
using System.Runtime.CompilerServices;

namespace Legion.ADF.Messaging.MessageBox;

public partial interface IMessageBoxUnitOfWork : Legion.Model.Repositories.IUnitOfWork, IDisposable, IAsyncDisposable
{

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IBlockedMessageTypeRepository BlockedMessageTypeRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageRepository MessageRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageArchiveRepository MessageArchiveRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageBoxInstanceRepository MessageBoxInstanceRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageBoxProcessingLogRepository MessageBoxProcessingLogRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageContentRepository MessageContentRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageProcessingLogRepository MessageProcessingLogRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageProcessingStatusRepository MessageProcessingStatusRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageStatusRepository MessageStatusRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IMessageTypeRepository MessageTypeRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueueRepository QueueRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueuedMessageRepository QueuedMessageRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IQueueProcessingModeRepository QueueProcessingModeRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.ISubscribedMessageRepository SubscribedMessageRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.ITopicRepository TopicRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.ITopicSubscriptionRepository TopicSubscriptionRepository { get; }
}
