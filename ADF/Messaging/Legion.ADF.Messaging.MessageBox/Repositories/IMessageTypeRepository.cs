namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageTypeRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageType>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.MessageType.IGetAllMessageTypes GetAllMessageTypes(
		Legion.ADF.Messaging.MessageBox.Queries.MessageType.GetAllMessageTypesQuery getAllMessageTypes);

	Legion.ADF.Messaging.MessageBox.Queries.MessageType.IGetMessageTypeByNamespace GetMessageTypeByNamespace(
		Legion.ADF.Messaging.MessageBox.Queries.MessageType.GetMessageTypeByNamespaceQuery getMessageTypeByNamespace);
}
