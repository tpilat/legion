namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwMessageContentRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageContent>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwMessageContent.IGetVwMessageContentById GetVwMessageContentById(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageContent.GetVwMessageContentByIdQuery getVwMessageContentByIdMessage);
}
