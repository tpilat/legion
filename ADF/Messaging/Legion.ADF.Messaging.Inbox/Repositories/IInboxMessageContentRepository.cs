namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxMessageContentRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageContent>? AccessControlManager { get; }

}
