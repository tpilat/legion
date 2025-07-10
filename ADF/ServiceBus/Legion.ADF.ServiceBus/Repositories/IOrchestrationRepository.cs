namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.Orchestration>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.Orchestration>? AccessControlManager { get; }

}
