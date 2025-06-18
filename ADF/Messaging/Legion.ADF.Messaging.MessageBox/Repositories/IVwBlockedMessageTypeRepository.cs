namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwBlockedMessageTypeRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwBlockedMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType.IGetAllVwBlockedMessageTypes GetAllVwBlockedMessageTypes(
		Legion.ADF.Messaging.MessageBox.Queries.VwBlockedMessageType.GetAllVwBlockedMessageTypesQuery getAllVwBlockedMessageTypes);
}
