namespace Legion.ADF.ServiceBus.Jobs;

public partial interface IJobsRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IJobsRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IJobsRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
