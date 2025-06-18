namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobDataRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobData>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobData>? AccessControlManager { get; }

}
