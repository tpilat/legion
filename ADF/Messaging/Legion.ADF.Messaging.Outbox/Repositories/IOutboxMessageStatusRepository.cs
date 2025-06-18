namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxMessageStatusRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageStatus>? AccessControlManager { get; }

}
