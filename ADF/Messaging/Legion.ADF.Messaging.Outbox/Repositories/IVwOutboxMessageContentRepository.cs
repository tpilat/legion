namespace Legion.ADF.Messaging.Outbox.Model.Repositories;

public partial interface IVwOutboxMessageContentRepository : Legion.ADF.Messaging.Outbox.IOutboxQueryRepository<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Outbox.Model.VwOutboxMessageContent>? AccessControlManager { get; }

	Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent.IGetVwOutboxMessageContentById GetVwOutboxMessageContentById(
		Legion.ADF.Messaging.Outbox.Queries.VwOutboxMessageContent.GetVwOutboxMessageContentByIdQuery getVwOutboxMessageContentByIdMessage);
}
