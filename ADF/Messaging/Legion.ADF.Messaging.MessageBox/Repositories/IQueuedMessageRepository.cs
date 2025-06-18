namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IQueuedMessageRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.QueuedMessage>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.IGetNextQueuedMessagesByQueue GetNextQueuedMessagesByQueue(
		Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetNextQueuedMessagesByQueueQuery getNextQueuedMessagesByQueue);

	Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.IGetQueuedMessageById GetQueuedMessageById(
		Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessageByIdQuery getQueuedMessageByIdQuery);

	Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.IGetQueuedMessagesByIdMessage GetQueuedMessagesByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.QueuedMessage.GetQueuedMessagesByIdMessageQuery getQueuedMessagesByIdMessage);
}
