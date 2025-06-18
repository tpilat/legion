namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface ISubscribedMessageRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.SubscribedMessage>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.IGetNextSubscribedMessagesBySubscription GetNextSubscribedMessagesBySubscription(
		Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetNextSubscribedMessagesBySubscriptionQuery getNextSubscribedMessagesBySubscription);

	Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.IGetSubscribedMessageById GetSubscribedMessageById(
		Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessageByIdQuery getSubscribedMessageById);

	Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.IGetSubscribedMessagesByIdMessage GetSubscribedMessagesByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.SubscribedMessage.GetSubscribedMessagesByIdMessageQuery getSubscribedMessagesByIdMessage);
}
