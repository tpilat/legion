namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IOutboxMessageArchiveRepository : Legion.ADF.Messaging.Outbox.IOutboxRepository<Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.OutboxMessageArchive>? AccessControlManager { get; }

}
