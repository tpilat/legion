namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IHostRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.Host>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.Host>? AccessControlManager { get; }

	Legion.ADF.ServiceBus.Queries.Host.IGetAllHosts GetAllHosts(
		Legion.ADF.ServiceBus.Queries.Host.GetAllHostsQuery getAllHosts);

	Legion.ADF.ServiceBus.Queries.Host.IGetHostById GetHostById(
		Legion.ADF.ServiceBus.Queries.Host.GetHostByIdQuery getHostById);

	Legion.ADF.ServiceBus.Queries.Host.IGetHostByName GetHostByName(
		Legion.ADF.ServiceBus.Queries.Host.GetHostByNameQuery getHostByName);
}
