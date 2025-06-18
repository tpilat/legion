namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxMessageStatusRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageStatus>? AccessControlManager { get; }

}
