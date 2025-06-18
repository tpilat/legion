namespace Legion.ADF.Audit;

public partial interface IAuditQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface IAuditQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, IAuditQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
