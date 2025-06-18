namespace Legion.ADF.Logs.Model.Repositories;

public partial interface ILogRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.Log>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.Log>? AccessControlManager { get; }

	Legion.ADF.Logs.Queries.Log.IGetLogById GetLogById(
		Legion.ADF.Logs.Queries.Log.GetLogByIdQuery getLogByIdQuery);
}
