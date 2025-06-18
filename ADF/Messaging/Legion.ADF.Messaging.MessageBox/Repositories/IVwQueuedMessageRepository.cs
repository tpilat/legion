namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwQueuedMessageRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwQueuedMessage>? AccessControlManager { get; }

}
