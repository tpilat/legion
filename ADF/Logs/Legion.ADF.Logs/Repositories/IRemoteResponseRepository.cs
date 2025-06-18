namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IRemoteResponseRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.RemoteResponse>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteResponse>? AccessControlManager { get; }

}
