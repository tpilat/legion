namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IQueueRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.Queue>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.Queue>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.Queue.IGetAllQueues GetAllQueues(
		Legion.ADF.Messaging.MessageBox.Queries.Queue.GetAllQueuesQuery getAllQueues);

	Legion.ADF.Messaging.MessageBox.Queries.Queue.IGetAllQueuesByEvents GetAllQueuesByEvents(
		Legion.ADF.Messaging.MessageBox.Queries.Queue.GetAllQueuesByEventsQuery getAllQueuesByEvents);

	Legion.ADF.Messaging.MessageBox.Queries.Queue.IGetQueueByName GetQueueByName(
		Legion.ADF.Messaging.MessageBox.Queries.Queue.GetQueueByNameQuery getQueueByName);
}
