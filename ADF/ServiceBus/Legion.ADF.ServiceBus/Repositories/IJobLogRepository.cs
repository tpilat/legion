namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobLogRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobLog>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobLog>? AccessControlManager { get; }

	Legion.ADF.ServiceBus.Queries.JobLog.IGetJobLogsByIdJob GetJobLogsByIdJob(
		Legion.ADF.ServiceBus.Queries.JobLog.GetJobLogsByIdJobQuery getJobLogsByIdJobQuery);
}
