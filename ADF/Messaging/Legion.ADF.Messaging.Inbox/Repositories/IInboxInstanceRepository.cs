namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxInstanceRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxInstance>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxInstance>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.InboxInstance.IExistsInboxInstanceById ExistsInboxInstanceById(
		Legion.ADF.Messaging.Inbox.Queries.InboxInstance.ExistsInboxInstanceByIdQuery existsInboxInstanceById);

	Legion.ADF.Messaging.Inbox.Queries.InboxInstance.IGetInboxInstanceById GetInboxInstanceById(
		Legion.ADF.Messaging.Inbox.Queries.InboxInstance.GetInboxInstanceByIdQuery getInboxInstanceById);
}
