namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IVwInboxQueueMessagesRepository : Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxQueueMessages>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage.IGetAllInboxQueues GetAllInboxQueues(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxQueueMessage.GetAllInboxQueuesQuery getAllInboxQueues);
}
