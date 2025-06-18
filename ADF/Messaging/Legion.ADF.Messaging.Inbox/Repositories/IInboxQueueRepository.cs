namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxQueueRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxQueue>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxQueue>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.InboxQueue.IGetAllInboxQueues GetAllInboxQueues(
		Legion.ADF.Messaging.Inbox.Queries.InboxQueue.GetAllInboxQueuesQuery getAllInboxQueues);

	Legion.ADF.Messaging.Inbox.Queries.InboxQueue.IGetAllInboxQueuesByEvents GetAllInboxQueuesByEvents(
		Legion.ADF.Messaging.Inbox.Queries.InboxQueue.GetAllInboxQueuesByEventsQuery getAllInboxQueuesByEvents);

	Legion.ADF.Messaging.Inbox.Queries.InboxQueue.IGetInboxQueueByName GetInboxQueueByName(
		Legion.ADF.Messaging.Inbox.Queries.InboxQueue.GetInboxQueueByNameQuery getInboxQueueByName);
}
