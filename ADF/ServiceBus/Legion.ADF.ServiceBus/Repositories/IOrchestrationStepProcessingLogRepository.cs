namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationStepProcessingLogRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationStepProcessingLog>? AccessControlManager { get; }

}
