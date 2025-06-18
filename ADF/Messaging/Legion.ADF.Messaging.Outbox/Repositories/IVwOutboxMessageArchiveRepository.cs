namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IVwOutboxMessageArchiveRepository : Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageArchive>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.IGetAllVwOutboxMessageArchivesByIdQueue GetAllVwOutboxMessageArchivesByIdQueue(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetAllVwOutboxMessageArchivesByIdQueueQuery getAllVwOutboxMessageArchivesByIdQueue);

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.IGetVwOutboxMessageArchiveByIdMessage GetVwOutboxMessageArchiveByIdMessage(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageArchive.GetVwOutboxMessageArchiveByIdMessageQuery getVwOutboxMessageArchiveByIdMessage);
}
