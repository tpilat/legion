namespace Legion.ADF.ServiceBus.Orchestrations.Model.Repositories;

public partial interface IOrchestrationRepository : Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository<Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.Orchestration>? AccessControlManager { get; }

}
