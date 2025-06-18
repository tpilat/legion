namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxProcessingLogRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxProcessingLog>? AccessControlManager { get; }

}
