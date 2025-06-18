namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxMessageTypeRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.IGetAllOutboxMessageTypes GetAllOutboxMessageTypes(
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.GetAllOutboxMessageTypesQuery getAllOutboxMessageTypes);

	Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.IGetOutboxMessageTypeByNamespace GetOutboxMessageTypeByNamespace(
		Legion.ADF.Messaging.Outbox.Queries.OutboxMessageType.GetOutboxMessageTypeByNamespaceQuery getOutboxMessageTypeByNamespace);
}
