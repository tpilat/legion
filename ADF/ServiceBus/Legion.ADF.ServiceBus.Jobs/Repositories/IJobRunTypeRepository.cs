namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobRunTypeRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobRunType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobRunType>? AccessControlManager { get; }

}
