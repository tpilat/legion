namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageStatusRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageStatus>? AccessControlManager { get; }

}
