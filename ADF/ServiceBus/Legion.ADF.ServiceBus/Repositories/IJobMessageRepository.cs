namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobMessageRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobMessage>? AccessControlManager { get; }

}
