namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IEventCounterRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.EventCounter>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.EventCounter>? AccessControlManager { get; }

}
