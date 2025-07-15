namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobActivityRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobActivity>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobActivity>? AccessControlManager { get; }

}
