namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxMessageRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessage>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.InboxMessage.IExistsInboxMessageByQueueMessageId ExistsInboxMessageByQueueMessageId(
		Legion.ADF.Messaging.Inbox.Queries.InboxMessage.ExistsInboxMessageByQueueMessageIdQuery existsInboxMessageByQueueMessageId);

	Legion.ADF.Messaging.Inbox.Queries.InboxMessage.IGetInboxMessageById GetInboxMessageById(
		Legion.ADF.Messaging.Inbox.Queries.InboxMessage.GetInboxMessageByIdQuery getInboxMessageById);

	Legion.ADF.Messaging.Inbox.Queries.InboxMessage.IGetNextInboxMessagesByQueue GetNextInboxMessagesByQueue(
		Legion.ADF.Messaging.Inbox.Queries.InboxMessage.GetNextInboxMessagesByQueueQuery getNextInboxMessagesByQueue);
}
