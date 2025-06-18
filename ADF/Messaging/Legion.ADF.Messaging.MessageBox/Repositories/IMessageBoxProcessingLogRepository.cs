namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageBoxProcessingLogRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageBoxProcessingLog>? AccessControlManager { get; }

}
