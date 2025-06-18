namespace Legion.ADF.ServiceBus.Orchestrations.Model.Repositories;

public partial interface IVwOrchestrationRepository : Legion.ADF.ServiceBus.Orchestrations.IOrchestrationsQueryRepository<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Orchestrations.Model.VwOrchestration>? AccessControlManager { get; }

}
