namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IVwInboxMessageRepository : Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessage>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.IGetAllVwInboxMessagesByIdQueue GetAllVwInboxMessagesByIdQueue(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetAllVwInboxMessagesByIdQueueQuery getAllVwInboxMessagesByIdQueue);

	Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.IGetVwInboxMessageByIdMessage GetVwInboxMessageByIdMessage(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessage.GetVwInboxMessageByIdMessageQuery getVwInboxMessageByIdMessage);
}
