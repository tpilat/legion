namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IMessageArchiveRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxRepository<Legion.ADF.Messaging.MessageBox.Model.MessageArchive>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.MessageArchive>? AccessControlManager { get; }

}
