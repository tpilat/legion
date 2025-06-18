namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwQueueRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwQueue>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwQueue>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwQueue.IGetVwQueueById GetVwQueueById(
		Legion.ADF.Messaging.MessageBox.Queries.VwQueue.GetVwQueueByIdQuery getVwQueueById);
}
