namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IVwInboxMessageArchiveRepository : Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageArchive>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.IGetAllVwInboxMessageArchivesByIdQueue GetAllVwInboxMessageArchivesByIdQueue(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetAllVwInboxMessageArchivesByIdQueueQuery getAllVwInboxMessageArchivesByIdQueue);

	Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.IGetVwInboxMessageArchiveByIdMessage GetVwInboxMessageArchiveByIdMessage(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageArchive.GetVwInboxMessageArchiveByIdMessageQuery getVwInboxMessageArchiveByIdMessage);
}
