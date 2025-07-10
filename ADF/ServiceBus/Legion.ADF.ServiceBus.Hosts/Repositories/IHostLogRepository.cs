namespace Legion.ADF.ServiceBus.Hosts.Model.Repositories;

public partial interface IHostLogRepository : Legion.ADF.ServiceBus.Hosts.IHostsRepository<Legion.ADF.ServiceBus.Hosts.Model.HostLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Hosts.Model.HostLog>? AccessControlManager { get; }

}
