namespace Legion.ADF.Logs.Model.Repositories;

public partial interface ILogLevelRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.LogLevel>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.LogLevel>? AccessControlManager { get; }

}
