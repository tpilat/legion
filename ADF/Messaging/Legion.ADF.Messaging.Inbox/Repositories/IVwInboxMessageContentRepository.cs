namespace Legion.ADF.Messaging.Inbox.Model.Repositories;

public partial interface IVwInboxMessageContentRepository : Legion.ADF.Messaging.Inbox.IInboxQueryRepository<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.Inbox.Model.VwInboxMessageContent>? AccessControlManager { get; }

	Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent.IGetVwInboxMessageContentById GetVwInboxMessageContentById(
		Legion.ADF.Messaging.Inbox.Queries.VwInboxMessageContent.GetVwInboxMessageContentByIdQuery getVwInboxMessageContentByIdMessage);
}
