namespace Legion.ADF.ServiceBus.Orchestrations.Model.Repositories;

public partial interface IOrchestrationStatusRepository : Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStatus>? AccessControlManager { get; }

}
