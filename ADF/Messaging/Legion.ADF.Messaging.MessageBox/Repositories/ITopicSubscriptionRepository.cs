namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface ITopicSubscriptionRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.TopicSubscription>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetAllTopicSubscriptions GetAllTopicSubscriptions(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsQuery getAllTopicSubscriptions);

	Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetAllTopicSubscriptionsByTopic GetAllTopicSubscriptionsByTopic(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicQuery getAllTopicSubscriptionsByTopic);

	Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetAllTopicSubscriptionsByTopicAndEvents GetAllTopicSubscriptionsByTopicAndEvents(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetAllTopicSubscriptionsByTopicAndEventsQuery getAllTopicSubscriptionsByTopicAndEvents);

	Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.IGetTopicSubscriptionByTopicAndName GetTopicSubscriptionByTopicAndName(
		Legion.ADF.Messaging.MessageBox.Queries.TopicSubscription.GetTopicSubscriptionByTopicAndNameQuery getTopicSubscriptionByTopicAndName);
}
