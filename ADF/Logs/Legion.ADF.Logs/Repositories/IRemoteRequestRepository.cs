namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IRemoteRequestRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.RemoteRequest>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteRequest>? AccessControlManager { get; }

}
