namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IJobStatisticsRepository : Legion.ADF.ServiceBus.Jobs.IJobsRepository<Legion.ADF.ServiceBus.Jobs.Model.JobStatistics>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.JobStatistics>? AccessControlManager { get; }

}
