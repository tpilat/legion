namespace Legion.ADF.Logs.Model.Repositories;

public partial interface ILocalRequestPayloadRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.LocalRequestPayload>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.LocalRequestPayload>? AccessControlManager { get; }

}
