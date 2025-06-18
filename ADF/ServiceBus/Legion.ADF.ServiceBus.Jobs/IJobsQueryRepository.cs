namespace Legion.ADF.ServiceBus.Jobs;

public partial interface IJobsQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IJobsQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IJobsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
