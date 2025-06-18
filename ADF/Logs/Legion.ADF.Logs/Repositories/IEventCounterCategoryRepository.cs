namespace Legion.ADF.Logs.Model.Repositories;

public partial interface IEventCounterCategoryRepository : Legion.ADF.Logs.ILogsRepository<Legion.ADF.Logs.Model.EventCounterCategory>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.Logs.Model.EventCounterCategory>? AccessControlManager { get; }

}
