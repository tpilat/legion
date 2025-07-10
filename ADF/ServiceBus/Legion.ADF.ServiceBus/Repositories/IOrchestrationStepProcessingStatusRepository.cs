namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationStepProcessingStatusRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingStatus>? AccessControlManager { get; }

}
