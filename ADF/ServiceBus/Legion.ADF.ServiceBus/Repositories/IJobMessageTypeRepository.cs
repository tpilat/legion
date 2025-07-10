namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobMessageTypeRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobMessageType>? AccessControlManager { get; }

}
