namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobExecutionRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobExecution>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobExecution>? AccessControlManager { get; }

}
