namespace Legion.ADF.ServiceBus.Hosts;

public partial interface IHostsQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IHostsQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IHostsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
