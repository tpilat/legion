namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IVwOutboxQueueRepository : Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxQueue>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue.IGetVwOutboxQueueById GetVwOutboxQueueById(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxQueue.GetVwOutboxQueueByIdQuery getVwOutboxQueueById);
}
