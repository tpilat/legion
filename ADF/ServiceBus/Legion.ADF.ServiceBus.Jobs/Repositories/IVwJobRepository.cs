namespace Legion.ADF.ServiceBus.Jobs.Model.Repositories;

public partial interface IVwJobRepository : Legion.ADF.ServiceBus.Jobs.IJobsQueryRepository<Legion.ADF.ServiceBus.Jobs.Model.VwJob>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Jobs.Model.VwJob>? AccessControlManager { get; }

}
