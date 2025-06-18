namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxProcessingLogRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxProcessingLog>? AccessControlManager { get; }

}
