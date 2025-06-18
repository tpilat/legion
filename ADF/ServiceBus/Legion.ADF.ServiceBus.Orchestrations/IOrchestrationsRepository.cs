namespace Legion.ADF.ServiceBus.Orchestrations;

public partial interface IOrchestrationsRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IOrchestrationsRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IOrchestrationsRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
