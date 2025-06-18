namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IBlockedMessageTypeRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.BlockedMessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.BlockedMessageType.IGetAllBlockedMessageTypes GetAllBlockedMessageTypes(
		Legion.ADF.Messaging.MessageBox.Queries.BlockedMessageType.GetAllBlockedMessageTypesQuery getAllBlockedMessageTypes);

	Legion.ADF.Messaging.MessageBox.Queries.BlockedMessageType.IGetBlockedMessageTypesByNamespaces GetBlockedMessageTypesByNamespaces(
		Legion.ADF.Messaging.MessageBox.Queries.BlockedMessageType.GetBlockedMessageTypesByNamespacesQuery GetBlockedMessageTypesByNamespaces);
}
