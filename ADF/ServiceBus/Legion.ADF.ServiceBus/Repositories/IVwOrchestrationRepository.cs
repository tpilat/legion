namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IVwOrchestrationRepository : Legion.ADF.ServiceBus.IServiceBusQueryRepository<Legion.ADF.ServiceBus.Model.VwOrchestration>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.VwOrchestration>? AccessControlManager { get; }

}
