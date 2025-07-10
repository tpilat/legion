namespace Legion.ADF.ServiceBus;

public partial interface IServiceBusRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IServiceBusRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IServiceBusRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
