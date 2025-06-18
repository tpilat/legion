namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IRemoteResponsePayloadRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.RemoteResponsePayload>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteResponsePayload>? AccessControlManager { get; }

}
