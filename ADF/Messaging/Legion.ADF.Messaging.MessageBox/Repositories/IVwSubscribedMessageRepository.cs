namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwSubscribedMessageRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwSubscribedMessage>? AccessControlManager { get; }

}
