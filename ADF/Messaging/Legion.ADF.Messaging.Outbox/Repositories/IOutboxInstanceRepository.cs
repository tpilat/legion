namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxInstanceRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxInstance>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxInstance>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.OutboxInstance.IExistsOutboxInstanceById ExistsOutboxInstanceById(
		Legion.ADF.Messaging.Outbox.Queries.OutboxInstance.ExistsOutboxInstanceByIdQuery existsOutboxInstanceById);

	Legion.ADF.Messaging.Outbox.Queries.OutboxInstance.IGetOutboxInstanceById GetOutboxInstanceById(
		Legion.ADF.Messaging.Outbox.Queries.OutboxInstance.GetOutboxInstanceByIdQuery getOutboxInstanceById);
}
