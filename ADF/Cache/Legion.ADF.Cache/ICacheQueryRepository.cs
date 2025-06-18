namespace Legion.ADF.Cache;

public partial interface ICacheQueryRepository : Legion.Model.Repositories.IQueryRepositoryBase
{
}

public interface ICacheQueryRepository<T> : Legion.Model.Repositories.IQueryRepositoryBase<T>, ICacheQueryRepository, Legion.Model.Repositories.IQueryRepositoryBase
{
}
