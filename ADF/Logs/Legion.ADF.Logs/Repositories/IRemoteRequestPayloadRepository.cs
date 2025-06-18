namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IRemoteRequestPayloadRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.RemoteRequestPayload>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.RemoteRequestPayload>? AccessControlManager { get; }

}
