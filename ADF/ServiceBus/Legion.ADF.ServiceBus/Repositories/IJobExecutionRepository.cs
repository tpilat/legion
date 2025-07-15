namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobExecutionRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobExecution>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobExecution>? AccessControlManager { get; }

	Legion.ADF.ServiceBus.Queries.JobExecution.IGetJobExecutionById GetJobExecutionById(
		Legion.ADF.ServiceBus.Queries.JobExecution.GetJobExecutionByIdQuery getJobExecutionById);
}
