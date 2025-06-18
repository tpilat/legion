namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IUnstructuredLogRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.UnstructuredLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.UnstructuredLog>? AccessControlManager { get; }

	Legion.ADF.Logs.Queries.UnstructuredLog.IGetUnstructuredLogById GetUnstructuredLogById(
		Legion.ADF.Logs.Queries.UnstructuredLog.GetUnstructuredLogByIdQuery getUnstructuredLogByIdQuery);
}
