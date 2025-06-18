namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobMessageRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobMessage>? AccessControlManager { get; }

}
