namespace Legion.ADF.ESB.Orchestrations;

public partial interface IOrchestrationsQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IOrchestrationsQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IOrchestrationsQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
