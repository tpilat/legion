namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IRemoteSystemRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.RemoteSystem>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteSystem>? AccessControlManager { get; }

}
