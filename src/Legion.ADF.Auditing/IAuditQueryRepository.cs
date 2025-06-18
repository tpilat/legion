using Legion.Model.Repositories;

namespace Legion.ADF.Auditing;

public partial interface IAuditQueryRepository : IQueryRepositoryBase
{
}

public interface IAuditQueryRepository<T> : IQueryRepositoryBase<T>, IAuditQueryRepository, IQueryRepositoryBase
{
}
