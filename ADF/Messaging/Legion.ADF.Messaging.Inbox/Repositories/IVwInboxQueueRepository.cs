namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IVwInboxQueueRepository : Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxQueue>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxQueue>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.VwInboxQueue.IGetVwInboxQueueById GetVwInboxQueueById(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxQueue.GetVwInboxQueueByIdQuery getVwInboxQueueById);
}
