namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobLogRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobLog>? AccessControlManager { get; }

}
