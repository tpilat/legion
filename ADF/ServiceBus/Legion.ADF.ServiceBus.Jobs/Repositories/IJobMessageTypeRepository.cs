namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobMessageTypeRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobMessageType>? AccessControlManager { get; }

}
