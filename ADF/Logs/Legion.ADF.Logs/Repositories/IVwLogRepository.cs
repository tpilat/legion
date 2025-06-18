namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IVwLogRepository : Legion.ADF.Logs.ILogsQueryRepository<Legion.ADF.Logs.Model.VwLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.VwLog>? AccessControlManager { get; }

}
