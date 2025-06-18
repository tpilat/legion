namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.Message>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.Message>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.Message.IExistsMessageByQueueMessageId ExistsMessageByQueueMessageId(
		Legion.ADF.Messaging.MessageBox.Queries.Message.ExistsMessageByQueueMessageIdQuery existsMessageByQueueMessageId);

	Legion.ADF.Messaging.MessageBox.Queries.Message.IExistsMessageByTopicMessageId ExistsMessageByTopicMessageId(
		Legion.ADF.Messaging.MessageBox.Queries.Message.ExistsMessageByTopicMessageIdQuery existsMessageByTopicMessageId);

	Legion.ADF.Messaging.MessageBox.Queries.Message.IGetMessageById GetMessageById(
		Legion.ADF.Messaging.MessageBox.Queries.Message.GetMessageByIdQuery getMessageById);
}
