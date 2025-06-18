namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IQueueProcessingModeRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.QueueProcessingMode>? AccessControlManager { get; }

}
