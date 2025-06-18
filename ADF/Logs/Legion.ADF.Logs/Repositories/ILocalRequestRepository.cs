namespace Legion.ADF.Logs.Model.Repositories;

public partial interface ILocalRequestRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.LocalRequest>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.LocalRequest>? AccessControlManager { get; }

}
