namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageProcessingLogRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageProcessingLog>? AccessControlManager { get; }

}
