namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageProcessingStatusRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingStatus>? AccessControlManager { get; }

}
