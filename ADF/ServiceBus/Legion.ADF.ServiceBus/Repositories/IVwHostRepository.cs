namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IVwHostRepository : Legion.ADF.ServiceBus.IServiceBusQueryRepository<Legion.ADF.ServiceBus.Model.VwHost>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.VwHost>? AccessControlManager { get; }

}
