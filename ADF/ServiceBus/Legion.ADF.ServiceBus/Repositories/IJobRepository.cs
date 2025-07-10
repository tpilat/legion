namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.Job>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.Job>? AccessControlManager { get; }

}
