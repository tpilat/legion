namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface ITopicRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.Topic>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.Topic>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.Topic.IGetAllTopics GetAllTopics(
		Legion.ADF.Messaging.MessageBox.Queries.Topic.GetAllTopicsQuery getAllTopics);

	Legion.ADF.Messaging.MessageBox.Queries.Topic.IGetAllTopicsByNames GetAllTopicsByNames(
		Legion.ADF.Messaging.MessageBox.Queries.Topic.GetAllTopicsByNamesQuery getAllTopicsByNames);

	Legion.ADF.Messaging.MessageBox.Queries.Topic.IGetTopicByName GetTopicByName(
		Legion.ADF.Messaging.MessageBox.Queries.Topic.GetTopicByNameQuery getTopicByName);
}
