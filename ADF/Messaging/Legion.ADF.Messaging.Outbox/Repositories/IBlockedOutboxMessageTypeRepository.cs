namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IBlockedOutboxMessageTypeRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.BlockedOutboxMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.IGetAllBlockedOutboxMessageTypes GetAllBlockedOutboxMessageTypes(
		Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetAllBlockedOutboxMessageTypesQuery getAllBlockedOutboxMessageTypes);

	Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.IGetBlockedOutboxMessageTypesByNamespaces GetBlockedOutboxMessageTypesByNamespaces(
		Legion.ADF.Messaging.Outbox.Queries.BlockedOutboxMessageType.GetBlockedOutboxMessageTypesByNamespacesQuery GetBlockedOutboxMessageTypesByNamespaces);
}
