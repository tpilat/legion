namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationStepProcessingMessageRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingMessage>? AccessControlManager { get; }

}
