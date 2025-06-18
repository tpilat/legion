namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxMessageRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessage>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.IExistsOutboxMessageByQueueMessageId ExistsOutboxMessageByQueueMessageId(
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.ExistsOutboxMessageByQueueMessageIdQuery existsOutboxMessageByQueueMessageId);

	Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.IGetNextOutboxMessagesByQueue GetNextOutboxMessagesByQueue(
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.GetNextOutboxMessagesByQueueQuery getNextOutboxMessagesByQueue);

	Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.IGetOutboxMessageById GetOutboxMessageById(
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessage.GetOutboxMessageByIdQuery getOutboxMessageById);
}
