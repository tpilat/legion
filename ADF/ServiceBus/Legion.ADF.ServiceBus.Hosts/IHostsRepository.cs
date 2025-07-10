namespace Legion.ADF.ServiceBus.Hosts;

public partial interface IHostsRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IHostsRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IHostsRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
