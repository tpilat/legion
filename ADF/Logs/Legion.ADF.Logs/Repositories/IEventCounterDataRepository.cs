namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IEventCounterDataRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.EventCounterData>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.EventCounterData>? AccessControlManager { get; }

}
