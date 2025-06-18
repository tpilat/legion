namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxMessageContentRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageContent>? AccessControlManager { get; }

}
