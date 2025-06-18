namespace Legion.ADF.Audit;

public partial interface IAuditRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface IAuditRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, IAuditRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
