namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobDataRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobData>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobData>? AccessControlManager { get; }

}
