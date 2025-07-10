namespace Legion.ADF.ServiceBus.Model.Repositories;

public partial interface IJobRunTypeRepository : Legion.ADF.ServiceBus.IServiceBusRepository<Legion.ADF.ServiceBus.Model.JobRunType>
{
	Legion.ACL.IAccessControlManager<Legion.ADF.ServiceBus.Model.JobRunType>? AccessControlManager { get; }

}
