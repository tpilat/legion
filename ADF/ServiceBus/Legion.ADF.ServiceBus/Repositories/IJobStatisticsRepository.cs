namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobStatisticsRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobStatistics>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobStatistics>? AccessControlManager { get; }

	Legion.ADF.ServiceBus.Queries.JobStatistics.IGetJobStatisticsByJobId GetJobStatisticsByJobId(
		Legion.ADF.ServiceBus.Queries.JobStatistics.GetJobStatisticsByJobIdQuery getJobStatisticsById);

	Legion.ADF.ServiceBus.Queries.JobStatistics.IGetJobStatisticsByJobIdAndStartHour GetJobStatisticsByJobIdAndStartHour(
		Legion.ADF.ServiceBus.Queries.JobStatistics.GetJobStatisticsByJobIdAndStartHourQuery getJobStatisticsByIdAndStartHour);
}
