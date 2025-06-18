namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobStatusRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobStatus>? AccessControlManager { get; }

}
