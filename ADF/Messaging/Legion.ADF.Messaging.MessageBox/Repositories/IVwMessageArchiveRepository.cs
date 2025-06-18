namespace Legion.ADF.Messaging.MessageBox.Model.Repositories;

public partial interface IVwMessageArchiveRepository : Legion.ADF.Messaging.MessageBox.IMessageBoxQueryRepository<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Messaging.MessageBox.Model.VwMessageArchive>? AccessControlManager { get; }

	Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.IGetAllVwMessageArchivesByIdQueue GetAllVwMessageArchivesByIdQueue(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.GetAllVwMessageArchivesByIdQueueQuery getAllVwMessageArchivesByIdQueue);

	Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.IGetVwMessageArchiveByIdMessage GetVwMessageArchiveByIdMessage(
		Legion.ADF.Messaging.MessageBox.Queries.VwMessageArchive.GetVwMessageArchiveByIdMessageQuery getVwMessageArchiveByIdMessage);
}
