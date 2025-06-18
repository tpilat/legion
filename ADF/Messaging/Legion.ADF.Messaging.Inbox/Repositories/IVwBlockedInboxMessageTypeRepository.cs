namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IVwBlockedInboxMessageTypeRepository : Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwBlockedInboxMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType.IGetAllVwBlockedInboxMessageTypes GetAllVwBlockedInboxMessageTypes(
		Legion.ADF.Messaging.Inbox.Queries.VwBlockedInboxMessageType.GetAllVwBlockedInboxMessageTypesQuery getAllVwBlockedInboxMessageTypes);
}
