namespace Legion.ADF.ServiceBus.Orchestrations.Model.Repositories;

public partial interface IOrchestrationStepProcessingStatusRepository : Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessingStatus>? AccessControlManager { get; }

}
