namespace Legion.ADF.Cache;

public partial interface ICacheRepository : Legion.Model.Repositories.IEntityRepositoryBase
{
}

public interface ICacheRepository<T> : Legion.Model.Repositories.IEntityRepositoryBase<T>, ICacheRepository, Legion.Model.Repositories.IEntityRepositoryBase
{
}
