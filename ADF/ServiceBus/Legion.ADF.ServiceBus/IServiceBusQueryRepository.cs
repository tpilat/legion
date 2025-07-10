namespace Legion.ADF.ServiceBus;

public partial interface IServiceBusQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IServiceBusQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IServiceBusQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
