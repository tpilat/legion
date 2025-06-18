namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IVwOutboxMessageProcessingLogRepository : Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageProcessingLog>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog.IGetVwOutboxMessageProcessingLogsByIdMessage GetVwOutboxMessageProcessingLogsByIdMessage(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageProcessingLog.GetVwOutboxMessageProcessingLogsByIdMessageQuery getVwOutboxMessageProcessingLogByIdMessage);
}
