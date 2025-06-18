namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageContentRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageContent>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageContent>? AccessControlManager { get; }

}
