namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationStepRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationStep>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationStep>? AccessControlManager { get; }

}
