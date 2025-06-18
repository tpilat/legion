namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwTopicRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwTopic>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwTopic>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwTopic.IGetVwTopicById GetVwTopicById(
		Legion.ADF.Messaging.MessageBox.Queries.VwTopic.GetVwTopicByIdQuery getVwTopicById);
}
