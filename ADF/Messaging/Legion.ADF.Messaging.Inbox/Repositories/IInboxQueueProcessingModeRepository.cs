namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxQueueProcessingModeRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxQueueProcessingMode>? AccessControlManager { get; }

}
