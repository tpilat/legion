namespace Legion.ADF.Logs.Model.Repositories;

public partial interface ILocalResponsePayloadRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.LocalResponsePayload>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.LocalResponsePayload>? AccessControlManager { get; }

}
