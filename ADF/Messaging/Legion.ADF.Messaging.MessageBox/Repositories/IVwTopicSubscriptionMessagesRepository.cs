namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwTopicSubscriptionMessagesRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscriptionMessages>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage.IGetAllTopicSubscriptions GetAllTopicSubscriptions(
		Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscriptionMessage.GetAllTopicSubscriptionsQuery getAllTopicSubscriptions);
}
