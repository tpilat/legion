namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationStepProcessingRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessing>? AccessControlManager { get; }

}
