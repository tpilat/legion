namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwQueueMessagesRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwQueueMessages>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage.IGetAllQueues GetAllQueues(
		Legion.ADF.Messaging.MessageBox.Queries.VwQueueMessage.GetAllQueuesQuery getAllQueues);
}
