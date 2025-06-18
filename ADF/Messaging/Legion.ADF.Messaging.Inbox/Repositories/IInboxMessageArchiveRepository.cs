namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxMessageArchiveRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageArchive>? AccessControlManager { get; }

}
