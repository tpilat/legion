namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobStatisticsRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobStatistics>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobStatistics>? AccessControlManager { get; }

}
