namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IBlockedInboxMessageTypeRepository : Legion.ADF.Messaging.Inbox.IInboxRepository<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.BlockedInboxMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.IGetAllBlockedInboxMessageTypes GetAllBlockedInboxMessageTypes(
		Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetAllBlockedInboxMessageTypesQuery getAllBlockedInboxMessageTypes);

	Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.IGetBlockedInboxMessageTypesByNamespaces GetBlockedInboxMessageTypesByNamespaces(
		Legion.ADF.Messaging.Inbox.Queries.BlockedInboxMessageType.GetBlockedInboxMessageTypesByNamespacesQuery GetBlockedInboxMessageTypesByNamespaces);
}
