namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.Job>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.Job>? AccessControlManager { get; }

	Legion.ADF.ServiceBus.Queries.Job.IGetAllJobs GetAllJobs(
		Legion.ADF.ServiceBus.Queries.Job.GetAllJobsQuery getAllJobs);

	Legion.ADF.ServiceBus.Queries.Job.IGetJobById GetJobById(
		Legion.ADF.ServiceBus.Queries.Job.GetJobByIdQuery getJobById);
}
