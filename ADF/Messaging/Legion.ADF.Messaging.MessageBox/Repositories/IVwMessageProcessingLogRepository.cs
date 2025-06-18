namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwMessageProcessingLogRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageProcessingLog>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog.IGetVwMessageProcessingLogsByIdMessage GetVwMessageProcessingLogsByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageProcessingLog.GetVwMessageProcessingLogsByIdMessageQuery getVwMessageProcessingLogByIdMessage);
}
