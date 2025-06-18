namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IVwBlockedOutboxMessageTypeRepository : Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwBlockedOutboxMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType.IGetAllVwBlockedOutboxMessageTypes GetAllVwBlockedOutboxMessageTypes(
		Legion.ADF.Messaging.Outbox.Queries.VwBlockedOutboxMessageType.GetAllVwBlockedOutboxMessageTypesQuery getAllVwBlockedOutboxMessageTypes);
}
