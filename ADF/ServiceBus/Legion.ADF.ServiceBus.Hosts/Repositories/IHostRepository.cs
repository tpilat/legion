namespace Legion.ADF.ServiceBus.Hosts.Model.Repositories;

public partial interface IHostRepository : Legion.ADF.ServiceBus.Hosts.IHostsRepository<Legion.ADF.ServiceBus.Hosts.Model.Host>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Hosts.Model.Host>? AccessControlManager { get; }

	Legion.ADF.ServiceBus.Hosts.Queries.Host.IGetHostById GetHostById(
		Legion.ADF.ServiceBus.Hosts.Queries.Host.GetHostByIdQuery getHostById);

	Legion.ADF.ServiceBus.Hosts.Queries.Host.IGetHostByName GetHostByName(
		Legion.ADF.ServiceBus.Hosts.Queries.Host.GetHostByNameQuery getHostByName);
}
