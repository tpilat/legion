namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IVwInboxMessageProcessingLogRepository : Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageProcessingLog>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog.IGetVwInboxMessageProcessingLogsByIdMessage GetVwInboxMessageProcessingLogsByIdMessage(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageProcessingLog.GetVwInboxMessageProcessingLogsByIdMessageQuery getVwInboxMessageProcessingLogByIdMessage);
}
