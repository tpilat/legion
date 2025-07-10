namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IVwJobRepository : Legion.ADF.ServiceBus.IServiceBusQueryRepository<Legion.ADF.ServiceBus.Model.VwJob>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.VwJob>? AccessControlManager { get; }

}
