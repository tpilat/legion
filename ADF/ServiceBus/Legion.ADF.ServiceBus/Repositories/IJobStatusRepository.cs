namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobStatusRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobStatus>? AccessControlManager { get; }

}
