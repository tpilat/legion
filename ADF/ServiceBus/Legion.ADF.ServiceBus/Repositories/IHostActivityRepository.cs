namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IHostActivityRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.HostActivity>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.HostActivity>? AccessControlManager { get; }

}
