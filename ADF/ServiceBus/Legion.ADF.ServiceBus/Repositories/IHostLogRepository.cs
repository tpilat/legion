namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IHostLogRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.HostLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.HostLog>? AccessControlManager { get; }

	Legion.ADF.ServiceBus.Queries.HostLog.IGetHostLogsByIdHost GetHostLogsByIdHost(
		Legion.ADF.ServiceBus.Queries.HostLog.GetHostLogsByIdHostQuery getHostLogsByIdHostQuery);
}
