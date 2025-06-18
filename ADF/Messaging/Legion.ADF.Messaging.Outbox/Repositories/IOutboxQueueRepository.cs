namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxQueueRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxQueue>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxQueue>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.IGetAllOutboxQueues GetAllOutboxQueues(
		Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.GetAllOutboxQueuesQuery getAllOutboxQueues);

	Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.IGetAllOutboxQueuesByEvents GetAllOutboxQueuesByEvents(
		Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.GetAllOutboxQueuesByEventsQuery getAllOutboxQueuesByEvents);

	Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.IGetOutboxQueueByName GetOutboxQueueByName(
		Legion.ADF.Messaging.Outbox.Queries.OutboxQueue.GetOutboxQueueByNameQuery getOutboxQueueByName);
}
