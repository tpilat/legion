namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IInboxMessageTypeRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.InboxMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.InboxMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.InboxMessageType.IGetAllInboxMessageTypes GetAllInboxMessageTypes(
		Legion.ADF.Messaging.Inbox.Queries.InboxMessageType.GetAllInboxMessageTypesQuery getAllInboxMessageTypes);

	Legion.ADF.Messaging.Inbox.Queries.InboxMessageType.IGetInboxMessageTypeByNamespace GetInboxMessageTypeByNamespace(
		Legion.ADF.Messaging.Inbox.Queries.InboxMessageType.GetInboxMessageTypeByNamespaceQuery getInboxMessageTypeByNamespace);
}
