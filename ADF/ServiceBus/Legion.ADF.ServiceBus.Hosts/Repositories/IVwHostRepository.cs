namespace Legion.ADF.ServiceBus.Hosts.Model.Repositories;

public partial interface IVwHostRepository : Legion.ADF.ServiceBus.Hosts.IHostsQueryRepository<Legion.ADF.ServiceBus.Hosts.Model.VwHost>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Hosts.Model.VwHost>? AccessControlManager { get; }

}
