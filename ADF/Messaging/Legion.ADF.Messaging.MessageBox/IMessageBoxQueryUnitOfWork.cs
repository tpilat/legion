namespace Legion.ADF.Messaging.MessageBox;

public partial interface IMessageBoxQueryUnitOfWork : Legion.Model.Repositories.IQueryUnitOfWork, IDisposable, IAsyncDisposable
{
	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwBlockedMessageTypeRepository VwBlockedMessageTypeRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageRepository VwMessageRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageArchiveRepository VwMessageArchiveRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageContentRepository VwMessageContentRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwMessageProcessingLogRepository VwMessageProcessingLogRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueueRepository VwQueueRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueuedMessageRepository VwQueuedMessageRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwQueueMessagesRepository VwQueueMessagesRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwSubscribedMessageRepository VwSubscribedMessageRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicRepository VwTopicRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicSubscriptionRepository VwTopicSubscriptionRepository { get; }

	Legion.ADF.Messaging.MessageBox.Model.Repositories.IVwTopicSubscriptionMessagesRepository VwTopicSubscriptionMessagesRepository { get; }
}
