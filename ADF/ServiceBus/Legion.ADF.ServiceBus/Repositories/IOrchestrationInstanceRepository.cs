namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationInstanceRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationInstance>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationInstance>? AccessControlManager { get; }

}
