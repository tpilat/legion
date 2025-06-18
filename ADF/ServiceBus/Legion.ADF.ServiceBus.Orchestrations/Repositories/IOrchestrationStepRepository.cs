namespace Legion.ADF.ServiceBus.Orchestrations.Model.Repositories;

public partial interface IOrchestrationStepRepository : Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsRepository<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.OrchestrationStep>? AccessControlManager { get; }

}
