namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationStepProcessingMessageTypeRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessageType>? AccessControlManager { get; }

}
