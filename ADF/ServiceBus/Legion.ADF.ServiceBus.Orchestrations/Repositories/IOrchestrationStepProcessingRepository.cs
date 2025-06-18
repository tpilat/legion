namespace Legion.ADF.ServiceBus.Orchestrations.Model.Repositories;

public partial interface IOrchestrationStepProcessingRepository : Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStepProcessing>? AccessControlManager { get; }

}
