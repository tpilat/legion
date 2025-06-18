namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.Job>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.Job>? AccessControlManager { get; }

}
