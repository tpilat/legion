namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IVwOutboxMessageRepository : Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessage>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.IGetAllVwOutboxMessagesByIdQueue GetAllVwOutboxMessagesByIdQueue(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.GetAllVwOutboxMessagesByIdQueueQuery getAllVwOutboxMessagesByIdQueue);

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.IGetVwOutboxMessageByIdMessage GetVwOutboxMessageByIdMessage(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessage.GetVwOutboxMessageByIdMessageQuery getVwOutboxMessageByIdMessage);
}
