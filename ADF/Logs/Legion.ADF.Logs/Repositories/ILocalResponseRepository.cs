namespace Legion.ADF.Logs.Model.Repositories;

public partial interface ILocalResponseRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.LocalResponse>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.LocalResponse>? AccessControlManager { get; }

}
