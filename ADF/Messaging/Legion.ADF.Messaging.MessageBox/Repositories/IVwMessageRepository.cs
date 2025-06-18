namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwMessageRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessage>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwMessage.IGetAllVwMessagesByIdQueue GetAllVwMessagesByIdQueue(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetAllVwMessagesByIdQueueQuery getAllVwMessagesByIdQueue);

	Legion.ADF.Messaging.MessageBox.Queries.VwMessage.IGetVwMessageByIdMessage GetVwMessageByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessage.GetVwMessageByIdMessageQuery getVwMessageByIdMessage);
}
