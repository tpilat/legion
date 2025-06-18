namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxMessageProcessingLogRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageProcessingLog>? AccessControlManager { get; }

}
