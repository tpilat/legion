namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxQueueProcessingModeRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxQueueProcessingMode>? AccessControlManager { get; }

}
