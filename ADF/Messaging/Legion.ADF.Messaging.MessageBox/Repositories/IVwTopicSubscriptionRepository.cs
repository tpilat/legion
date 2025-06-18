namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwTopicSubscriptionRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopicSubscription>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscription.IGetVwTopicSubscriptionById GetVwTopicSubscriptionById(
		Legion.ADF.Messaging.MessageBox.Queries.VwTopicSubscription.GetVwTopicSubscriptionByIdQuery getVwTopicSubscriptionById);
}
