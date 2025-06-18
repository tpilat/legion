namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IVwOutboxQueueMessagesRepository : Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueueMessages>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage.IGetAllOutboxQueues GetAllOutboxQueues(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueueMessage.GetAllOutboxQueuesQuery getAllOutboxQueues);
}
