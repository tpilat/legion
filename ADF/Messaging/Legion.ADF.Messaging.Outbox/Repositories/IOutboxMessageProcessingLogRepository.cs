namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxMessageProcessingLogRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageProcessingLog>? AccessControlManager { get; }

}
