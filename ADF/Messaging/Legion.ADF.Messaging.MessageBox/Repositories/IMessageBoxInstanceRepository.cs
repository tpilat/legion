namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageBoxInstanceRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageBoxInstance>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance.IExistsMessageBoxInstanceById ExistsMessageBoxInstanceById(
		Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance.ExistsMessageBoxInstanceByIdQuery existsMessageBoxInstanceById);

	Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance.IGetMessageBoxInstanceById GetMessageBoxInstanceById(
		Legion.ADF.Messaging.MessageBox.Queries.MessageBoxInstance.GetMessageBoxInstanceByIdQuery getMessageBoxInstanceById);
}
