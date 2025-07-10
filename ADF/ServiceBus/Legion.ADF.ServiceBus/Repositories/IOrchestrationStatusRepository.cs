namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IOrchestrationStatusRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.OrchestrationStatus>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.OrchestrationStatus>? AccessControlManager { get; }

}
