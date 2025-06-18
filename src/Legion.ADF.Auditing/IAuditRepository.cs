using Legion.Model.Repositories;

namespace Legion.ADF.Auditing;

public partial interface IAuditRepository : IEntityRepositoryBase
{
}

public interface IAuditRepository<T> : IEntityRepositoryBase<T>, IAuditRepository, IEntityRepositoryBase
{
}
